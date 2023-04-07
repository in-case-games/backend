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

### Информация о предметах
Получение всех предметов:
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/game-item`

**STATUS CODE 200**
```JSON
{
  "success": true,
  "data": [
    {
      "name": "M4A1-S",
      "cost": 100,
      "imageUri": "string",
      "idForPlatform": "string",
      "quality": {
        "name": "well worn",
        "id": "527316fc-5eed-44d2-8b25-2458e0d0b648"
      },
      "type": {
        "name": "weapon",
        "id": "f8a8acce-5f41-4417-b6c2-09c11039bc22"
      },
      "rarity": {
        "name": "white",
        "id": "a94fb5be-e114-4816-84dc-5acd485e2fc9"
      },
      "id": "47908aed-2372-4965-92f9-0498515aaadb"
    }
  ]
}
```
Получение одного предмета:
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/game-item/{id}`

**STATUS CODE 200**
```JSON
{
  "success": true,
  "data": {
    "name": "M4A1-S",
    "cost": 100,
    "imageUri": "string",
    "idForPlatform": "string",
    "quality": {
      "name": "well worn",
      "id": "527316fc-5eed-44d2-8b25-2458e0d0b648"
    },
    "type": {
      "name": "weapon",
      "id": "f8a8acce-5f41-4417-b6c2-09c11039bc22"
    },
    "rarity": {
      "name": "white",
      "id": "a94fb5be-e114-4816-84dc-5acd485e2fc9"
    },
    "id": "47908aed-2372-4965-92f9-0498515aaadb"
  }
}
```
**STATUS CODE 404**
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```

Получение всех качеств предмета:
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/game-item/qualities`

**STATUS CODE 200**
```JSON
{
  "success": true,
  "data": [
    {
      "name": "battle scarred",
      "id": "3b2bb9d1-2399-4a5a-bb31-8d0da5996d80"
    },
    {
      "name": "factory new",
      "id": "87b871c7-8c3e-44bf-b699-1c75dfab1174"
    },
    {
      "name": "field tested",
      "id": "41f61cdc-2486-472e-8520-02ff4ae6c11d"
    },
    {
      "name": "minimal wear",
      "id": "adfd38d3-86fc-4dd9-8bfa-20f62a48af83"
    },
    {
      "name": "none",
      "id": "3a54b65d-d585-48a2-a058-de2e271d4bec"
    },
    {
      "name": "well worn",
      "id": "527316fc-5eed-44d2-8b25-2458e0d0b648"
    }
  ]
}
```

Получение всех типов предмета:
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/game-item/types`

**STATUS CODE 200**
```JSON
{
  "success": true,
  "data": [
    {
      "name": "knife",
      "id": "feb91174-e508-45c8-b816-29cf9d95c745"
    },
    {
      "name": "none",
      "id": "076656e8-c989-4cf5-bdf4-f010f617adc6"
    },
    {
      "name": "pistol",
      "id": "afd4d497-9708-4cf4-b40c-bce2d4b286e9"
    },
    {
      "name": "rifle",
      "id": "8818b2fe-47d9-43f1-aea4-c6c41bbc7633"
    },
    {
      "name": "weapon",
      "id": "f8a8acce-5f41-4417-b6c2-09c11039bc22"
    }
  ]
}
```
Получение всех редкостей предмета:
* Доступ: Allow Anonymous
* Метод: GET 
* Запрос: `https://r.api.incase.com/api/game-item/rarities`

**STATUS CODE 200**
```JSON
{
  "success": true,
  "data": [
    {
      "name": "blue",
      "id": "21bda5e4-8f36-45cb-a6ed-0cd856eae509"
    },
    {
      "name": "gold",
      "id": "46ef8135-00bd-4e4e-8048-dda1525bb29b"
    },
    {
      "name": "pink",
      "id": "d1030028-0987-4a52-893f-489976a9eb42"
    },
    {
      "name": "red",
      "id": "cc27336a-f435-4af3-a6bf-02ce48db156e"
    },
    {
      "name": "violet",
      "id": "011eb185-5575-4bcc-8ce6-a3c2ec63bdef"
    },
    {
      "name": "white",
      "id": "a94fb5be-e114-4816-84dc-5acd485e2fc9"
    }
  ]
}
```
Создание нового предмета:
* Доступ: Admin, Owner, Bot
* Метод: POST 
* Запрос: `https://r.api.incase.com/api/game-item`

**Request body**
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "M4A1-S",
  "cost": 100,
  "imageUri": "string",
  "idForPlatform": "string",
  "gameId": "219c5443-9aba-4603-a595-f2f0ef382a39",
  "typeId": "f8a8acce-5f41-4417-b6c2-09c11039bc22",
  "rarityId": "a94fb5be-e114-4816-84dc-5acd485e2fc9",
  "qualityId": "527316fc-5eed-44d2-8b25-2458e0d0b648"
}
```

**STATUS CODE 200**
```JSON
{
  "success": true,
  "data": {
    "name": "M4A1-S",
    "cost": 100,
    "imageUri": "string",
    "idForPlatform": "string",
    "quality": null,
    "type": null,
    "rarity": null,
    "id": "47908aed-2372-4965-92f9-0498515aaadb"
  }
}
```

Обновление предмета:
* Доступ: Admin, Owner, Bot
* Метод: PUT 
* Запрос: `https://r.api.incase.com/api/game-item`

**Request body**
```JSON
{
  "id": "47908aed-2372-4965-92f9-0498515aaadb",
  "name": "M4A1-S",
  "cost": 150,
  "imageUri": "string",
  "idForPlatform": "string",
  "gameId": "219c5443-9aba-4603-a595-f2f0ef382a39",
  "typeId": "f8a8acce-5f41-4417-b6c2-09c11039bc22",
  "rarityId": "a94fb5be-e114-4816-84dc-5acd485e2fc9",
  "qualityId": "527316fc-5eed-44d2-8b25-2458e0d0b648"
}
```

**STATUS CODE 200**
```JSON
{
  "success": true,
  "data": {
    "name": "M4A1-S",
    "cost": 150,
    "imageUri": "string",
    "idForPlatform": "string",
    "quality": null,
    "type": null,
    "rarity": null,
    "id": "47908aed-2372-4965-92f9-0498515aaadb"
  }
}
```

**STATUS CODE 404**
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```
Удаление предмета:
* Доступ: Admin, Owner, Bot
* Метод: Delete
* Запрос: `https://r.api.incase.com/api/game-item/{id}`

**STATUS CODE 202**
```JSON
{
  "success": true,
  "data": "GameItem is succesfully removed"
}
```
**STATUS CODE 404**
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```
