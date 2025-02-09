# Калькулятор теории массового обслуживания

Система для расчета параметров очередей на основе моделей M/M/1 и M/M/c. Включает backend API на C# (.NET) и frontend на Vue.js.

## 🚀 Возможности

- Расчет параметров для систем с одним кассиром (M/M/1)
- Расчет параметров для систем с несколькими кассирами (M/M/c)
- Простой и понятный интерфейс
- Мгновенный расчет результатов
- Визуализация ключевых метрик

## 📋 Требования

### Backend
- .NET 9.0
- Visual Studio 2022, Visual Studio Code или Rider IDE

### Frontend
- Node.js 16.x или выше
- npm или yarn
- Vue.js 3.x

## 🛠 Установка и настройка

### Backend (API)

1. Клонируйте репозиторий:
```bash
git clone https://github.com/artemkiruhin/queuing-theory.git
cd queuing-theory/QueuingTheory.Api
```

2. Откройте решение в Visual Studio или Visual Studio Code

3. Восстановите пакеты NuGet:
```bash
dotnet restore
```

4. Запустите API:
```bash
dotnet run
```

API будет доступно по адресу: `http://localhost:5063`

### Frontend

1. Перейдите в папку frontend:
```bash
cd queuing-theory/frontend
```

2. Установите зависимости:
```bash
npm install
# или
yarn install
```

3. Настройте URL API в файле `.env`:
```
VITE_API_URL=http://localhost:5000
```

4. Запустите development сервер:
```bash
npm run dev
# или
yarn dev
```

Frontend будет доступен по адресу: `http://localhost:5173`

## 🔧 Конфигурация

### Backend

Настройка CORS в `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:5173") // Укажите ваш домен
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
```

### Frontend

Настройка API URL в `.env`:
```env
VITE_API_URL=http://your-api-url
```

## 📱 Использование

1. Откройте приложение в браузере
2. Выберите тип системы:
   - "Один кассир" для M/M/1
   - "Несколько кассиров" для M/M/c
3. Введите параметры:
   - Количество клиентов в час
   - Скорость обслуживания (клиентов в час на одного кассира)
   - Количество кассиров (для M/M/c)
4. Нажмите "Рассчитать"

### Интерпретация результатов

- **Время ожидания**: среднее время в очереди
- **Общее время**: полное время обслуживания (очередь + обслуживание)
- **Загруженность**: процент занятости кассиров
- **Очередь**: среднее количество людей в очереди

## 🚢 Развертывание

### Backend

1. Опубликуйте API:
```bash
dotnet publish -c Release
```

2. Разверните на вашем сервере (IIS, Linux + nginx, Azure, etc.)

### Frontend

1. Создайте production сборку:
```bash
npm run build
# или
yarn build
```

2. Разверните содержимое папки `dist` на веб-сервер

## 🔒 Безопасность

В production среде рекомендуется:

1. Настроить CORS для конкретных доменов
2. Использовать HTTPS
3. Добавить rate limiting для API
4. Настроить логирование

## 📝 API Документация

### Endpoints

#### POST /api/QueuingTheory/mm1
Расчет для системы с одним кассиром

Request:
```json
{
    "arrivalRate": 30,    // клиентов в час
    "serviceRate": 40     // клиентов в час
}
```

#### POST /api/QueuingTheory/mmc
Расчет для системы с несколькими кассирами

Request:
```json
{
    "arrivalRate": 30,    // клиентов в час
    "serviceRate": 40,    // клиентов в час
    "servers": 2          // количество кассиров
}
```

Response (для обоих endpoints):
```json
{
    "utilization": 0.75,  // загрузка системы (0-1)
    "l": 3.0,            // среднее число клиентов в системе
    "lq": 2.25,          // среднее число клиентов в очереди
    "w": 0.1,            // среднее время в системе (часы)
    "wq": 0.075,         // среднее время в очереди (часы)
    "p0": 0.25           // вероятность пустой системы
}
```

## 🤝 Участие в разработке

1. Форкните репозиторий
2. Создайте ветку для фичи (`git checkout -b feature/AmazingFeature`)
3. Зафиксируйте изменения (`git commit -m 'Add some AmazingFeature'`)
4. Отправьте изменения в ветку (`git push origin feature/AmazingFeature`)
5. Откройте Pull Request

## 📄 Лицензия

MIT License. См. файл `LICENSE` для деталей.
