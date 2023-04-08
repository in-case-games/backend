# Документация InCase.Resources.Api

**Содержание**

* Общее
  * Информация об играх
  * Информация о новостях
  * Информация о статистике сайта

* Предметы
  * Информация о предметах (предмете)
  * Информация о группах кейсов
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
  * Информация об отзывах
  * Информация об обращениях в тех поддержку
  
## Общее

_Данный подзаголовок содержит в себе информацио о том, как работать с общими endpoint-ами_


### Информация об играх

1. Получение всех игровых сущностей:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "name": "csgo",
      "items": [],
      "boxes": [],
      "platforms": null,
      "groups": null,
      "id": "bb3a297c-7d54-49ba-8906-7c8f5903a6ad"
    },
    {
      "name": "dota2",
      "items": [],
      "boxes": [],
      "platforms": null,
      "groups": null,
      "id": "4edd89d0-d4cf-4a58-93fb-a7b714510855"
    },
    {
      "name": "genshin",
      "items": [],
      "boxes": [],
      "platforms": null,
      "groups": null,
      "id": "d3de7704-031f-4b12-a7ad-c3bf42dfa0f1"
    }
  ]
}
```

2. Получение определенной игровой сущности: 
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data":
  {
    "name": "csgo",
    "items": [],
    "boxes": [],
    "platforms": null,
    "groups": null,
    "id": "bb3a297c-7d54-49ba-8906-7c8f5903a6ad"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Game is not found. "
}
```


### Информация о новостях

1. Получение всех новостей: 
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/news`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "title": "NEWS2",
      "date": "2023-04-08T05:01:03.28",
      "content": "BBBB",
      "images": [
        {
          "imageUri": "ЭТО ПУТЬ НА КАРТИНКУ",
          "id": "65576f0f-6a2c-4b22-aa5a-01a923707ac1"
        }
      ],
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ]
}
```

2. Получение определенной новости:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/news/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "NEWS2",
    "date": "2023-04-08T05:01:03.28",
    "content": "BBBB",
    "images": [
      {
        "imageUri": "ЭТО ПУТЬ НА КАРТИНКУ",
        "id": "65576f0f-6a2c-4b22-aa5a-01a923707ac1"
      }
    ],
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "News is not found. "
}
```

3. Создание новости:
   * Доступ: Admin, Owner, Bot
   * Метод: POST 
   * Запрос: `https://r.api.incase.com/api/news`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "NEWS",
  "date": "2023-04-08T04:26:24.822Z",
  "content": "AAAAA"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "NEWS",
    "date": "2023-04-08T04:26:24.822Z",
    "content": "AAAAA",
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

4. Обновление новости:
   * Доступ: Admin, Owner, Bot
   * Метод: PUT 
   * Запрос: `https://r.api.incase.com/api/news`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "NEWS2",
  "date": "2023-04-08T05:01:03.280Z",
  "content": "BBBB"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "NEWS2",
    "date": "2023-04-08T05:01:03.28Z",
    "content": "BBBB",
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "News is not found. "
}
```

5. Удаление новости:
   * Доступ: Admin, Owner, Bot
   * Метод: Delete 
   * Запрос: `https://r.api.incase.com/api/news`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "NEWS2",
    "date": "2023-04-08T05:01:03.28",
    "content": "BBBB",
    "images": null,
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

6. Создание картинки новости:
   * Доступ: Admin, Owner, Bot
   * Метод: POST 
   * Запрос: `https://r.api.incase.com/api/news/image`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "imageUri": "ЭТО ПУТЬ НА КАРТИНКУ",
  "newsId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "imageUri": "ЭТО ПУТЬ НА КАРТИНКУ",
    "news": null,
    "id": "24e26de0-adf0-43bb-ad04-5092cb543bda"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "NewsImage is not found. "
}
```

7. Удаление картинки новости:
   * Доступ: Admin, Owner, Bot
   * Метод: Delete 
   * Запрос: `https://r.api.incase.com/api/news/image/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "imageUri": "ЭТО ПУТЬ НА КАРТИНКУ",
    "id": "65576f0f-6a2c-4b22-aa5a-01a923707ac1"
  }
}
```


### Информация о статистике

1. Получение обычной статистике сайта:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/site-statistics`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

2. Получение админской статистике сайта:
   * Доступ: Owner, Bot
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/site-statistics/admin`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

3. Редактирование админской статистике сайта:
   * Доступ: Owner, Bot
   * Метод: PUT 
   * Запрос: `https://r.api.incase.com/api/site-statistics/admin`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "dcc228c3-6ad8-4021-9677-01b1067b5e23",
  "balanceWithdrawn": 110,
  "totalReplenished": 0,
  "sentSites": 0
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "balanceWithdrawn": 110,
    "totalReplenished": 0,
    "sentSites": 0,
    "id": "dcc228c3-6ad8-4021-9677-01b1067b5e23"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SiteStatisticsAdmin is not found. "
}
```

## Предметы

### Информация о предметах

1. Получение всех предметов:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

2. Получение одного предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```

3. Получение всех качеств предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/qualities`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

4. Получение всех типов предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/types`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

5. Получение всех редкостей предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/rarities`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

6. Создание нового предмета:
   * Доступ: Admin, Owner, Bot
   * Метод: POST 
   * Запрос: `https://r.api.incase.com/api/game-item`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

7. Обновление предмета:
   * Доступ: Admin, Owner, Bot
   * Метод: PUT 
   * Запрос: `https://r.api.incase.com/api/game-item`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=green)
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```

8. Удаление предмета:
   * Доступ: Admin, Owner, Bot
   * Метод: Delete
   * Запрос: `https://r.api.incase.com/api/game-item/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": "GameItem is succesfully removed"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```


### Информация о группах кейсов

1. Получение всех групп кейсов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group`

2. Получение группы кейса по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/{id}`

3. Получение всех групп:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/groups`

4. Создание группы кейсов:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box-group`

5. Создание группы:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box-group/group`

6. Удаление группы кейсов:
   * Доступ: Admin, Owner, Bot
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/loot-box-group/{id}`

7. Удаление группы:
   * Доступ: Admin, Owner, Bot
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/loot-box-group/group/{id}`


### Информация о кейсе

1. Получение всех кейсов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "name": "Самый классный кейс",
      "cost": 250,
      "balance": 0,
      "virtualBalance": 0,
      "imageUri": "string",
      "isLocked": false,
      "inventories": null,
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ]
}
```

2. Получение одного кейса:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "Самый классный кейс",
    "cost": 250,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string",
    "isLocked": false,
    "inventories": [],
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBox is not found. "
}
```

3. Получение содержимого кейса:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/{id}/inventory`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "numberItems": 1,
      "chanceWining": 120,
      "item": {
        "name": "НОЖ",
        "cost": 150,
        "imageUri": "string",
        "idForPlatform": "string",
        "quality": null,
        "type": null,
        "rarity": null,
        "id": "becb3051-91aa-4e52-b3f4-f26001afd8ed"
      },
      "id": "1174c0df-e11a-426d-a8ae-d8fba5acfe4d"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBox is not found. "
}
```

4. Получение всех баннеров кейсов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/banners`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "isActive": true,
      "creationDate": "2023-04-08T07:31:43.916",
      "expirationDate": "2023-04-08T07:31:43.916",
      "imageUri": "AAA",
      "box": {
        "name": "Самый классный кейс",
        "cost": 250,
        "balance": 0,
        "virtualBalance": 0,
        "imageUri": "string",
        "isLocked": false,
        "inventories": null,
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
      },
      "id": "6441d41f-c2cf-4c6c-91b0-52224cf5b4c3"
    }
  ]
}
```

5. Получение баннера у кейса:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/{id}/banner`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "isActive": true,
    "creationDate": "2023-04-08T07:31:43.916",
    "expirationDate": "2023-04-08T07:31:43.916",
    "imageUri": "AAA",
    "box": {
      "name": "Самый классный кейс",
      "cost": 250,
      "balance": 0,
      "virtualBalance": 0,
      "imageUri": "string",
      "isLocked": false,
      "inventories": null,
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    },
    "id": "6441d41f-c2cf-4c6c-91b0-52224cf5b4c3"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBoxBanner is not found. "
}
```

6. Создать новый кейс:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Самый классный кейс",
  "cost": 150,
  "balance": 0,
  "virtualBalance": 0,
  "imageUri": "string",
  "isLocked": false,
  "gameId": "219c5443-9aba-4603-a595-f2f0ef382a39"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "Самый классный кейс",
    "cost": 150,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string",
    "isLocked": false,
    "inventories": null,
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции INSERT с ограничением FOREIGN KEY \"fk_loot_box_game_game_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.Game\", column 'id'.\r\nВыполнение данной инструкции было прервано."
}
```

7. Обновить кейс:
   * Доступ: Admin, Owner, Bot
   * Метод: PUT
   * Запрос: `https://r.api.incase.com/api/loot-box`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Самый классный кейс",
  "cost": 250,
  "balance": 0,
  "virtualBalance": 0,
  "imageUri": "string",
  "isLocked": false,
  "gameId": "219c5443-9aba-4603-a595-f2f0ef382a39"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "Самый классный кейс",
    "cost": 250,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string",
    "isLocked": false,
    "inventories": null,
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции UPDATE с ограничением FOREIGN KEY \"fk_loot_box_game_game_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.Game\", column 'id'."
}
```

8. Создать новое содержимое кейсу:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box/inventory`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "numberItems": 1,
  "chanceWining": 120,
  "itemId": "becb3051-91aa-4e52-b3f4-f26001afd8ed",
  "boxId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "numberItems": 1,
    "chanceWining": 120,
    "item": null,
    "id": "1174c0df-e11a-426d-a8ae-d8fba5acfe4d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции INSERT с ограничением FOREIGN KEY \"fk_loot_box_inventory_game_item_item_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.GameItem\", column 'id'.\r\nВыполнение данной инструкции было прервано."
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции INSERT с ограничением FOREIGN KEY \"fk_loot_box_inventory_loot_box_box_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.LootBox\", column 'id'.\r\nВыполнение данной инструкции было прервано."
}
```

9. Создать баннер кейсу:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box/banner`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "isActive": true,
  "creationDate": "2023-04-08T07:29:07.297Z",
  "expirationDate": "2023-04-08T07:29:07.297Z",
  "imageUri": "ЭТО ПУТЬ К ФОТКЕ БАННЕРА",
  "boxId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "isActive": true,
    "creationDate": "2023-04-08T07:29:07.297Z",
    "expirationDate": "2023-04-08T07:29:07.297Z",
    "imageUri": "ЭТО ПУТЬ К ФОТКЕ БАННЕРА",
    "box": null,
    "id": "6441d41f-c2cf-4c6c-91b0-52224cf5b4c3"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции INSERT с ограничением FOREIGN KEY \"fk_loot_box_banner_loot_boxes_box_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.LootBox\", column 'id'.\r\nВыполнение данной инструкции было прервано."
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Не удается вставить повторяющуюся строку ключа в объект \"dbo.LootBoxBanner\" с уникальным индексом \"ix_loot_box_banner_box_id\". Повторяющееся значение ключа: (3fa85f64-5717-4562-b3fc-2c963f66afa6).\r\nВыполнение данной инструкции было прервано."
}
```

10. Обновить баннер кейса:
    * Доступ: Admin, Owner, Bot
    * Метод: PUT
    * Запрос: `https://r.api.incase.com/api/loot-box/banner`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "6441d41f-c2cf-4c6c-91b0-52224cf5b4c3",
  "isActive": true,
  "creationDate": "2023-04-08T07:31:43.916Z",
  "expirationDate": "2023-04-08T07:31:43.916Z",
  "imageUri": "AAA",
  "boxId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "isActive": true,
    "creationDate": "2023-04-08T07:29:07.297Z",
    "expirationDate": "2023-04-08T07:29:07.297Z",
    "imageUri": "ЭТО ПУТЬ К ФОТКЕ БАННЕРА",
    "box": null,
    "id": "6441d41f-c2cf-4c6c-91b0-52224cf5b4c3"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции UPDATE с ограничением FOREIGN KEY \"fk_loot_box_banner_loot_boxes_box_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.LootBox\", column 'id'."
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBoxBanner is not found. "
}
```

11. Удалить кейс:
    * Доступ: Admin, Owner, Bot
    * Метод: DELETE
    * Запрос: `https://r.api.incase.com/api/loot-box/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "Самый классный кейс",
    "cost": 250,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string",
    "isLocked": false,
    "inventories": null,
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBox is not found. "
}
```

12. Удалить баннер кейса:
    * Доступ: Admin, Owner, Bot
    * Метод: DELETE
    * Запрос: `https://r.api.incase.com/api/loot-box/banner/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "isActive": true,
    "creationDate": "2023-04-08T07:31:43.916",
    "expirationDate": "2023-04-08T07:31:43.916",
    "imageUri": "AAA",
    "box": null,
    "id": "6441d41f-c2cf-4c6c-91b0-52224cf5b4c3"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBoxBanner is not found. "
}
```

13. Удалить содержимое кейса:
    * Доступ: Admin, Owner, Bot
    * Метод: DELETE
    * Запрос: `https://r.api.incase.com/api/loot-box/inventory/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "numberItems": 1,
    "chanceWining": 120,
    "item": null,
    "id": "1174c0df-e11a-426d-a8ae-d8fba5acfe4d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBoxInventory is not found. "
}
```


### Информация о промокодах

1. Получить все промокоды:
   * Доступ: Admin, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/promocode`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "name": "string",
      "discount": 0,
      "numberActivations": 0,
      "expirationDate": "2023-04-08T08:15:30.484",
      "type": {
        "name": "balance",
        "id": "afce1b8c-8d16-4429-90d0-ae7959933063"
      },
      "id": "bbb36e4a-24f8-445f-8c6d-2df7e5d497ff"
    }
  ]
}
```

2. Получить один промокод по id:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/promocode/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "string",
    "discount": 0,
    "numberActivations": 0,
    "expirationDate": "2023-04-08T08:15:30.484",
    "type": {
      "name": "balance",
      "id": "afce1b8c-8d16-4429-90d0-ae7959933063"
    },
    "id": "bbb36e4a-24f8-445f-8c6d-2df7e5d497ff"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Promocode is not found. "
}
```

3. Получить один промокод по названию:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/promocode/name/{name}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "string",
    "discount": 0,
    "numberActivations": 0,
    "expirationDate": "2023-04-08T08:15:30.484",
    "type": {
      "name": "balance",
      "id": "afce1b8c-8d16-4429-90d0-ae7959933063"
    },
    "id": "bbb36e4a-24f8-445f-8c6d-2df7e5d497ff"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Promocode is not found. "
}
```

4. Получить типы промокодов:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/promocode/types`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "name": "balance",
      "id": "afce1b8c-8d16-4429-90d0-ae7959933063"
    },
    {
      "name": "case",
      "id": "de145f9a-522d-4e50-b427-c67fd1456e0e"
    }
  ]
}
```

5. Создать промокод:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/promocode`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "ЛУЧШИЙ-ПРОМОКОД",
  "discount": 10,
  "numberActivations": 1110,
  "expirationDate": "2023-04-08T08:12:17.749Z",
  "typeId": "afce1b8c-8d16-4429-90d0-ae7959933063"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "ЛУЧШИЙ-ПРОМОКОД",
    "discount": 10,
    "numberActivations": 1110,
    "expirationDate": "2023-04-08T08:12:17.749Z",
    "type": null,
    "id": "bbb36e4a-24f8-445f-8c6d-2df7e5d497ff"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Не удается вставить повторяющуюся строку ключа в объект \"dbo.Promocode\" с уникальным индексом \"ix_promocode_name\". Повторяющееся значение ключа: (ЛУЧШИЙ-ПРОМОКОД).\r\nВыполнение данной инструкции было прервано."
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Не удается вставить повторяющуюся строку ключа в объект \"dbo.Promocode\" с уникальным индексом \"ix_promocode_name\". Повторяющееся значение ключа: (string).\r\nВыполнение данной инструкции было прервано."
}
```

6. Обновить промокод:
   * Доступ: Admin, Owner, Bot
   * Метод: PUT
   * Запрос: `https://r.api.incase.com/api/promocode`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "bbb36e4a-24f8-445f-8c6d-2df7e5d497ff",
  "name": "string",
  "discount": 0,
  "numberActivations": 0,
  "expirationDate": "2023-04-08T08:15:30.484Z",
  "typeId": "afce1b8c-8d16-4429-90d0-ae7959933063"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "string",
    "discount": 0,
    "numberActivations": 0,
    "expirationDate": "2023-04-08T08:15:30.484Z",
    "type": null,
    "id": "bbb36e4a-24f8-445f-8c6d-2df7e5d497ff"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции UPDATE с ограничением FOREIGN KEY \"fk_promocode_promocode_types_type_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.PromocodeType\", column 'id'."
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Promocode is not found. "
}
```


7. Удалить промокод:
   * Доступ: Admin, Owner, Bot
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/promocode`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "string",
    "discount": 0,
    "numberActivations": 0,
    "expirationDate": "2023-04-08T08:15:30.484",
    "type": null,
    "id": "bbb36e4a-24f8-445f-8c6d-2df7e5d497ff"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Promocode is not found. "
}
```

## Пользователь

### Информация о пользователе

1. Получить дополнительную информацию:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-additional-info`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "balance": 0,
    "imageUri": "",
    "isNotifyEmail": false,
    "isGuestMode": false,
    "isConfirmed": true,
    "creationDate": "2023-04-07T05:52:48.4918144",
    "deletionDate": null,
    "role": {
      "name": "bot",
      "id": "7788cbe6-1a9e-41b0-91cd-f55fa1e60d5e"
    },
    "id": "7c1353fe-5582-403f-9c21-a32a2ab4f9a5"
  }
}
```

2. Получить все роли:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-additional-info/roles`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "name": "admin",
      "id": "2bd9949b-180e-4bed-9da2-3f0569cf157b"
    },
    {
      "name": "bot",
      "id": "7788cbe6-1a9e-41b0-91cd-f55fa1e60d5e"
    },
    {
      "name": "owner",
      "id": "c8c2854f-a142-4384-b424-acb7aa2928bf"
    },
    {
      "name": "support",
      "id": "f65c09b1-5ab4-4d9c-a10e-d41093f04146"
    },
    {
      "name": "user",
      "id": "37df21ec-8723-4ed2-957f-283ba1ecbb7c"
    }
  ]
}
```

3. Обновить дополнительную информацию о пользователе:
   * Доступ: Owner, Bot
   * Метод: PUT
   * Запрос: `https://r.api.incase.com/api/user-additional-info`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "7c1353fe-5582-403f-9c21-a32a2ab4f9a5",
  "balance": 1110,
  "imageUri": "",
  "isNotifyEmail": false,
  "isGuestMode": false,
  "isConfirmed": true,
  "creationDate": "2023-04-07T05:52:48.4918144",
  "deletionDate": null,
  "roleId": "7788cbe6-1a9e-41b0-91cd-f55fa1e60d5e",
  "userId": "753ed98a-cf5d-4acc-994c-afab92848fab"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "balance": 1110,
    "imageUri": "",
    "isNotifyEmail": false,
    "isGuestMode": false,
    "isConfirmed": true,
    "creationDate": "2023-04-07T05:52:48.4918144",
    "deletionDate": null,
    "role": null,
    "id": "7c1353fe-5582-403f-9c21-a32a2ab4f9a5"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции UPDATE с ограничением FOREIGN KEY \"fk_user_additional_info_user_roles_role_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.UserRole\", column 'id'."
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Конфликт инструкции UPDATE с ограничением FOREIGN KEY \"fk_user_additional_info_users_user_id\". Конфликт произошел в базе данных \"InCase.Dev\", таблица \"dbo.User\", column 'id'."
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserAdditionalInfo is not found. "
}
```
