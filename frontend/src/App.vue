<template>
  <div class="container mt-4">
    <h1 class="text-center mb-4">Калькулятор очередей</h1>

    <div class="card">
      <div class="card-body">
        <form @submit.prevent="calculate" class="mx-auto" style="max-width: 500px;">
          <!-- Простой выбор типа системы -->
          <div class="mb-4">
            <label class="form-label fw-bold">Тип системы</label>
            <select v-model="model" class="form-select form-select-lg">
              <option value="mm1">Один кассир (M/M/1)</option>
              <option value="mmc">Несколько кассиров (M/M/c)</option>
            </select>
          </div>

          <!-- Базовые параметры с подсказками -->
          <div class="mb-4">
            <label class="form-label fw-bold">
              Сколько клиентов приходит в час?
              <small class="text-muted d-block">Например: 30 клиентов в час</small>
            </label>
            <input
                type="number"
                class="form-control form-control-lg"
                v-model="params.arrivalRate"
                min="1"
                required
            >
          </div>

          <div class="mb-4">
            <label class="form-label fw-bold">
              Сколько клиентов может обслужить один кассир в час?
              <small class="text-muted d-block">Например: 40 клиентов в час</small>
            </label>
            <input
                type="number"
                class="form-control form-control-lg"
                v-model="params.serviceRate"
                min="1"
                required
            >
          </div>

          <div class="mb-4" v-if="model === 'mmc'">
            <label class="form-label fw-bold">
              Количество кассиров
              <small class="text-muted d-block">Например: 2 кассира</small>
            </label>
            <input
                type="number"
                class="form-control form-control-lg"
                v-model="params.servers"
                min="1"
                required
            >
          </div>

          <button type="submit" class="btn btn-primary btn-lg w-100">Рассчитать</button>
        </form>

        <!-- Упрощенные результаты -->
        <div v-if="results" class="mt-4">
          <h3 class="text-center mb-3">Результаты расчета</h3>
          <div class="row g-4">
            <div class="col-md-6">
              <div class="card h-100">
                <div class="card-body">
                  <h5 class="card-title">Время ожидания</h5>
                  <p class="card-text">
                    Клиент проведет в очереди около
                    <strong>{{ formatTime(results.wq) }}</strong>
                  </p>
                </div>
              </div>
            </div>
            <div class="col-md-6">
              <div class="card h-100">
                <div class="card-body">
                  <h5 class="card-title">Общее время</h5>
                  <p class="card-text">
                    Всё обслуживание займет около
                    <strong>{{ formatTime(results.w) }}</strong>
                  </p>
                </div>
              </div>
            </div>
            <div class="col-md-6">
              <div class="card h-100">
                <div class="card-body">
                  <h5 class="card-title">Загруженность</h5>
                  <p class="card-text">
                    Кассиры заняты
                    <strong>{{ formatPercent(results.utilization) }}%</strong> времени
                  </p>
                </div>
              </div>
            </div>
            <div class="col-md-6">
              <div class="card h-100">
                <div class="card-body">
                  <h5 class="card-title">Очередь</h5>
                  <p class="card-text">
                    В среднем в очереди
                    <strong>{{ formatNumber(results.lq) }}</strong> человек
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div v-if="error" class="alert alert-danger mt-3">
          {{ formatError(error) }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'App',
  data() {
    return {
      model: 'mm1',
      params: {
        arrivalRate: 30,
        serviceRate: 40,
        servers: 1
      },
      results: null,
      error: null
    }
  },
  methods: {
    async calculate() {
      try {
        this.error = null;
        const response = await axios.post(
            `http://localhost:5063/api/QueuingTheory/${this.model}`,
            this.params
        );
        this.results = response.data;
      } catch (err) {
        this.error = err.response?.data || 'Произошла ошибка при расчете';
        this.results = null;
      }
    },
    formatTime(hours) {
      const minutes = Math.round(hours * 60);
      if (minutes < 1) return 'меньше минуты';
      if (minutes === 1) return '1 минуту';
      if (minutes < 60) return `${minutes} минут`;
      return `${Math.floor(minutes / 60)} ч ${minutes % 60} мин`;
    },
    formatPercent(value) {
      return (value * 100).toFixed(0);
    },
    formatNumber(value) {
      if (value < 1) return 'менее 1';
      return value.toFixed(1);
    },
    formatError(error) {
      if (error.includes('unstable')) {
        return 'Система перегружена! Нужно либо увеличить скорость обслуживания, либо добавить кассиров.';
      }
      return 'Проверьте введенные данные и попробуйте снова.';
    }
  }
}
</script>