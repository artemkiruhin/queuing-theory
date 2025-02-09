<template>
  <div class="container py-5">
    <div class="row justify-content-center">
      <div class="col-lg-8">
        <div class="card shadow-lg border-0 rounded-lg">
          <div class="card-header bg-primary text-white text-center py-4">
            <h2 class="mb-0">Калькулятор очередей</h2>
            <p class="text-white-50 mb-0">Рассчитайте оптимальное время обслуживания</p>
          </div>

          <div class="card-body p-4">
            <form @submit.prevent="calculate">
              <!-- Тип системы -->
              <div class="mb-4">
                <label class="form-label h6">
                  <i class="bi bi-gear-fill me-2"></i>Тип системы
                </label>
                <select v-model="model" class="form-select form-select-lg border-0 bg-light">
                  <option value="mm1">Один кассир (M/M/1)</option>
                  <option value="mmc">Несколько кассиров (M/M/c)</option>
                </select>
              </div>

              <!-- Параметры -->
              <div class="row g-4">
                <div class="col-md-6">
                  <div class="form-floating">
                    <input
                        type="number"
                        class="form-control bg-light border-0"
                        id="arrivalRate"
                        v-model="params.arrivalRate"
                        min="1"
                        required
                        placeholder="Клиентов в час"
                    >
                    <label for="arrivalRate">Клиентов в час</label>
                  </div>
                  <small class="text-muted">Например: 30 клиентов в час</small>
                </div>

                <div class="col-md-6">
                  <div class="form-floating">
                    <input
                        type="number"
                        class="form-control bg-light border-0"
                        id="serviceRate"
                        v-model="params.serviceRate"
                        min="1"
                        required
                        placeholder="Обслуживание в час"
                    >
                    <label for="serviceRate">Обслуживание в час</label>
                  </div>
                  <small class="text-muted">Например: 40 клиентов в час</small>
                </div>

                <div class="col-12" v-if="model === 'mmc'">
                  <div class="form-floating">
                    <input
                        type="number"
                        class="form-control bg-light border-0"
                        id="servers"
                        v-model="params.servers"
                        min="1"
                        required
                        placeholder="Количество кассиров"
                    >
                    <label for="servers">Количество кассиров</label>
                  </div>
                  <small class="text-muted">Например: 2 кассира</small>
                </div>
              </div>

              <div class="d-grid gap-2 mt-4">
                <button type="submit" class="btn btn-primary btn-lg">
                  <i class="bi bi-calculator me-2"></i>Рассчитать
                </button>
              </div>
            </form>

            <!-- Результаты -->
            <div v-if="results" class="mt-5">
              <h4 class="text-center mb-4">Результаты расчета</h4>
              <div class="row g-4">
                <div class="col-md-6">
                  <div class="card h-100 border-0 bg-light">
                    <div class="card-body text-center p-4">
                      <i class="bi bi-clock-history display-4 text-primary mb-3"></i>
                      <h5 class="card-title">Время ожидания</h5>
                      <p class="card-text h4 text-primary">
                        {{ formatTime(results.wq) }}
                      </p>
                      <p class="text-muted small">в очереди</p>
                    </div>
                  </div>
                </div>

                <div class="col-md-6">
                  <div class="card h-100 border-0 bg-light">
                    <div class="card-body text-center p-4">
                      <i class="bi bi-clock display-4 text-primary mb-3"></i>
                      <h5 class="card-title">Общее время</h5>
                      <p class="card-text h4 text-primary">
                        {{ formatTime(results.w) }}
                      </p>
                      <p class="text-muted small">полное обслуживание</p>
                    </div>
                  </div>
                </div>

                <div class="col-md-6">
                  <div class="card h-100 border-0 bg-light">
                    <div class="card-body text-center p-4">
                      <i class="bi bi-person-workspace display-4 text-primary mb-3"></i>
                      <h5 class="card-title">Загруженность</h5>
                      <p class="card-text h4 text-primary">
                        {{ formatPercent(results.utilization) }}%
                      </p>
                      <p class="text-muted small">занятость кассиров</p>
                    </div>
                  </div>
                </div>

                <div class="col-md-6">
                  <div class="card h-100 border-0 bg-light">
                    <div class="card-body text-center p-4">
                      <i class="bi bi-people display-4 text-primary mb-3"></i>
                      <h5 class="card-title">Очередь</h5>
                      <p class="card-text h4 text-primary">
                        {{ formatNumber(results.lq) }}
                      </p>
                      <p class="text-muted small">человек в среднем</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div v-if="error" class="alert alert-danger mt-4 d-flex align-items-center">
              <i class="bi bi-exclamation-triangle-fill me-2"></i>
              {{ formatError(error) }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'QueueCalculator',
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