# Документация InCase.Resources.Api

**Содержание**

* Общее
  * Информация об играх
  * Информация о новостях
  * Информация о статистике сайта

* Предметы
  * Информация о предметах (предмете)
  * Информация о кейсе
  * Информация о промокодах

* Пользователь
  * Информация о пользователе (пользователях)
  * Информация об историях
    * Информация о пополнениях
    * Информация об открытиях
    * Информация об обменах
    * Информация о промокодах
  * Информация об ограничениях
  
## Общее

_Данный подзаголовок содержит в себе информацио о том, как работать с общими endpoint-ами_

### Информация об играх
Получение всех игровых сущностей: 
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/game`

**STATUS CODE 200:**
```JSON
{
  "success": true,
  "data": [
    {
      "name": "csgo",
      "items": [],
      "boxes": [],
      "platforms": null,
      "id": "bb3a297c-7d54-49ba-8906-7c8f5903a6ad"
    },
    {
      "name": "dota2",
      "items": [],
      "boxes": [],
      "platforms": null,
      "id": "4edd89d0-d4cf-4a58-93fb-a7b714510855"
    },
    {
      "name": "genshin",
      "items": [],
      "boxes": [],
      "platforms": null,
      "id": "d3de7704-031f-4b12-a7ad-c3bf42dfa0f1"
    }
  ]
}
```
Получение определенной игровой сущности: 
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/game/{id}`

**STATUS CODE 200:**
```JSON
{
  "success": true,
  "data":
  {
    "name": "csgo",
    "items": [],
    "boxes": [],
    "platforms": null,
    "id": "bb3a297c-7d54-49ba-8906-7c8f5903a6ad"
  }
}
```
**STATUS CODE 404:**
```JSON
{
  "success": false,
  "data": "Game is not found. "
}
```
### Информация о новостях
Получение всех новостей: 
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/news`

**STATUS CODE 200:**
```JSON
{
  "success": true,
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "title": "string",
      "date": "2023-04-06T22:56:18.774Z",
      "content": "string"
    }
  ]
}
```
Получение определенной новости:
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/news/{id}`

**STATUS CODE 200:**
```JSON
{
  "success": true,
  "data": 
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "string",
    "date": "2023-04-06T22:56:18.774Z",
    "content": "string"
  }
}
```
**STATUS CODE 404:**
```JSON
{
  "success": false,
  "data": "News is not found. "
}
```

### Информация о статистике
Получение обычной статистике сайта:
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/site-statistics`

**STATUS CODE 200:**
```JSON
{
  "success": true,
  "data": {
    "users": 0,
    "reviews": 0,
    "lootBoxes": 0,
    "withdrawnItems": 0,
    "withdrawnFunds": 0,
    "id": "b7d9b658-7116-46f2-85bf-91c09fbd316d"
  }
}
```
Получение админской статистике сайта:
* Доступ: Owner, Bot
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/site-statistics/admin`

**STATUS CODE 200:**
```JSON
{
  "success": true,
  "data": {
    "balanceWithdrawn": 0,
    "totalReplenished": 0,
    "sentSites": 0,
    "id": "dcc228c3-6ad8-4021-9677-01b1067b5e23"
  }
}
```
