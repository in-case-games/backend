# Документация InCase.Resources.Api

**Содержание**

* Общее
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-об-играх">Информация об играх</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-новостях">Информация о новостях</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-статистике">Информация о статистике сайта</a>**

* Предметы
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-предметах">Информация о предметах (предмете)</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-группах-кейсов">Информация о группах кейсов</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-кейсе">Информация о кейсе</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-промокодах">Информация о промокодах</a>**

* Пользователь
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-пользователе">Информация о пользователе (пользователях)</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-историях">Информация об историях</a>**
    * Информация о пополнениях
    * Информация об открытиях
    * Информация об обменах
    * Информация о промокодах
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-о-ограничениях">Информация об ограничениях</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-об-отзывах">Информация об отзывах</a>**
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-об-обращениях-в-техническую-поддержку">Информация об обращениях в тех поддержку</a>**
  
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Game is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItemType is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItemRarity is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItemQuality is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Game is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItemType is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItemRarity is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItemQuality is not found. "
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
  "data": {
    "name": "НОЖ",
    "cost": 150,
    "imageUri": "string",
    "idForPlatform": "string",
    "quality": null,
    "type": null,
    "rarity": null,
    "id": "becb3051-91aa-4e52-b3f4-f26001afd8ed"
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


### Информация о группах кейсов

1. Получение всех групп кейсов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "group": {
        "name": "string",
        "id": "1a97a421-16ed-4b96-a4b9-8ebfda7ae79c"
      },
      "box": {
        "name": "string",
        "cost": 0,
        "balance": 0,
        "virtualBalance": 0,
        "imageUri": "string",
        "isLocked": false,
        "inventories": null,
        "id": "38bd74d4-6b26-4725-84a7-b86b5c6703ad"
      },
      "id": "6966ff94-26af-429f-b4d3-afbdfd679481"
    }
  ]
}
```

2. Получение группы кейса по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "group": {
      "name": "string",
      "id": "1a97a421-16ed-4b96-a4b9-8ebfda7ae79c"
    },
    "box": {
      "name": "string",
      "cost": 0,
      "balance": 0,
      "virtualBalance": 0,
      "imageUri": "string",
      "isLocked": false,
      "inventories": null,
      "id": "38bd74d4-6b26-4725-84a7-b86b5c6703ad"
    },
    "id": "0743b1cf-63b8-4d92-9e50-d726bb0dbb61"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBoxGroup is not found. "
}
```

3. Получение всех групп:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/groups`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "name": "string",
      "id": "1a97a421-16ed-4b96-a4b9-8ebfda7ae79c"
    }
  ]
}
```

4. Создание группы кейсов:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box-group`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "boxId": "38bd74d4-6b26-4725-84a7-b86b5c6703ad",
  "groupId": "1a97a421-16ed-4b96-a4b9-8ebfda7ae79c",
  "gameId": "219c5443-9aba-4603-a595-f2f0ef382a39"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "group": null,
    "box": null,
    "id": "df514c21-c046-4abe-b7ed-3ca63299b5aa"
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GroupLootBox is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBox is not found. "
}
```

5. Создание группы:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box-group/group`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "string",
    "id": "01e72c7c-1738-4f81-8632-48203f8f6e67"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "The group name is already in use"
}
```


6. Удаление группы кейсов:
   * Доступ: Admin, Owner, Bot
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/loot-box-group/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "group": null,
    "box": null,
    "id": "bf73c90b-8df5-4940-8cb4-fa23da9536f1"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBoxGroup is not found. "
}
```

7. Удаление группы:
   * Доступ: Admin, Owner, Bot
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/loot-box-group/group/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "name": "string",
    "id": "1a97a421-16ed-4b96-a4b9-8ebfda7ae79c"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GroupLootBox is not found. "
}
```

7. Получить все группы по id игры:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/game/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "group": {
        "name": "string",
        "id": "1a97a421-16ed-4b96-a4b9-8ebfda7ae79c"
      },
      "box": {
        "name": "string",
        "cost": 0,
        "balance": 0,
        "virtualBalance": 0,
        "imageUri": "string",
        "isLocked": false,
        "inventories": null,
        "id": "38bd74d4-6b26-4725-84a7-b86b5c6703ad"
      },
      "id": "6966ff94-26af-429f-b4d3-afbdfd679481"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Game is not found. "
}
```

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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Game is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Game is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "GameItem is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBox is not found. "
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
  "data": "The banner is already used by this loot box"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBox is not found. "
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
  "data": "The banner is already used by this loot box"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "LootBox is not found. "
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
  "data": "The promocode name is already in use "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "PromocodeType is not found. "
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
  "data": "The promocode name is already in use "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "PromocodeType is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRole is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserAdditionalInfo is not found. "
}
```

4. Получить основную информацию:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "login": "GIS",
    "email": "yt_ferbray@mail.ru",
    "passwordHash": "upL6BXvpHm4TGjQ+BTkD1ll4jGgpkcvMMsmR1NpuBqc=",
    "passwordSalt": "JNpn50AEljepMvgrCLVqQGP+zwfUyCxB2Lzc9tTN1KCoceYRPAtzjMRQ9nGx4PGQ+6J1sL1PJrp6rUpSCo+Whw==",
    "additionalInfo": {
      "balance": 1110,
      "imageUri": "",
      "isNotifyEmail": false,
      "isGuestMode": false,
      "isConfirmed": true,
      "creationDate": "2023-04-07T05:52:48.4918144",
      "deletionDate": null,
      "role": null,
      "id": "7c1353fe-5582-403f-9c21-a32a2ab4f9a5"
    },
    "topics": null,
    "restrictions": null,
    "ownerRestrictions": null,
    "reviews": null,
    "historyPayments": null,
    "id": "753ed98a-cf5d-4acc-994c-afab92848fab"
  }
}
```

5. Получить основную информацию по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "login": "GIS",
    "email": "yt_ferbray@mail.ru",
    "passwordHash": null,
    "passwordSalt": null,
    "additionalInfo": {
      "balance": 1110,
      "imageUri": "",
      "isNotifyEmail": false,
      "isGuestMode": false,
      "isConfirmed": true,
      "creationDate": "2023-04-07T05:52:48.4918144",
      "deletionDate": null,
      "role": null,
      "id": "7c1353fe-5582-403f-9c21-a32a2ab4f9a5"
    },
    "topics": null,
    "restrictions": null,
    "ownerRestrictions": null,
    "reviews": null,
    "historyPayments": null,
    "id": "753ed98a-cf5d-4acc-994c-afab92848fab"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

6. Получить инвентарь пользователя:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/inventory`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```

7. Получить инвентарь пользователя по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

7. Получить путь к баннерам пользователя:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/banner`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```


### Информация о историях

1. Получить историю пополнения:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/history/payments`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```

2. Получить историю вывода:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/history/openings`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```

3. Получить историю вывода по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/{id}/history/withdrawns`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

4. Получить историю вывода:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/history/openings`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```

5. Получить историю промокодов:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user/history/promocodes`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```


### Информация о ограничениях

1. Получить ограничения пользователя по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-restriction/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "creationDate": "2023-04-08T12:06:07.209",
    "expirationDate": "2023-04-08T12:06:07.209",
    "description": "stringaaaa",
    "id": "1351f855-71c0-4ae1-963f-ca7bc73d6784"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
}
```

2. Получить все ограничения пользователя:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-restriction`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": []
}
```

3. Получить все ограничения пользователя по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-restriction/user/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "creationDate": "2023-04-08T12:06:07.209",
      "expirationDate": "2023-04-08T12:06:07.209",
      "description": "stringaaaa",
      "id": "1351f855-71c0-4ae1-963f-ca7bc73d6784"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

4. Получить все ограничения пользователя по id и по id обвинителя:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-restriction/{ownerId}&{userId}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "creationDate": "2023-04-08T12:27:21.729",
      "expirationDate": "2023-04-08T12:27:21.729",
      "description": "stringasdasdasd",
      "id": "46247306-910c-4ad8-86d7-8a738e6d5661"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

5. Получить все ограничения возложенные обвинителем:
   * Доступ: Admin, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-restriction/owner`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "creationDate": "2023-04-08T12:27:21.729",
      "expirationDate": "2023-04-08T12:27:21.729",
      "description": "stringasdasdasd",
      "id": "46247306-910c-4ad8-86d7-8a738e6d5661"
    }
  ]
}
```

6. Получить все ограничения возложенные обвинителем на пользователя по id:
   * Доступ: Admin, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-restriction/owner/{userId}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "creationDate": "2023-04-08T12:27:21.729",
      "expirationDate": "2023-04-08T12:27:21.729",
      "description": "stringasdasdasd",
      "id": "46247306-910c-4ad8-86d7-8a738e6d5661"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

7. Получить все типы ограничений:
   * Доступ: Admin, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-restriction/types`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "name": "ban",
      "id": "5c6c9522-36d1-4b43-b8bb-678aceac7c2b"
    },
    {
      "name": "mute",
      "id": "da9fe9b5-1a59-41aa-b574-553236e9a11c"
    },
    {
      "name": "warn",
      "id": "cdc27c2f-8bf6-4cfa-9c3f-021e80c600bf"
    }
  ]
}
```

8. Создать ограничение:
   * Доступ: Admin, Owner, Bot
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/user-restriction`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "creationDate": "2023-04-08T11:59:53.043Z",
    "expirationDate": "2023-04-08T11:59:53.043Z",
    "description": "НЕ СПАМЬ БОЛЬШЕ",
    "id": "3e206fd2-4bb2-42f4-8ea1-c39ab06436cf"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "RestrictionType is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
}
```

9. Обновить ограничение:
   * Доступ: Admin, Owner, Bot
   * Метод: PUT
   * Запрос: `https://r.api.incase.com/api/user-restriction`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "creationDate": "2023-04-08T12:06:07.209Z",
    "expirationDate": "2023-04-08T12:06:07.209Z",
    "description": "stringaaaa",
    "id": "1351f855-71c0-4ae1-963f-ca7bc73d6784"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "RestrictionType is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
}
```

10. Удалить ограничение:
    * Доступ: Admin, Owner, Bot
    * Метод: DELETE
    * Запрос: `https://r.api.incase.com/api/user-restriction`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "creationDate": "2023-04-08T12:27:21.729",
    "expirationDate": "2023-04-08T12:27:21.729",
    "description": "stringasdasdasd",
    "id": "46247306-910c-4ad8-86d7-8a738e6d5661"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
}
```


### Информация об отзывах

1. Получить все отзывы одобренные отзывы:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "title": "string",
      "content": "string",
      "isApproved": true,
      "images": [],
      "id": "80c5950d-07bc-445f-8808-e850cd0d6989"
    },
    {
      "title": "Лучший отзыв на свете",
      "content": "ПРошли нахуй пидоры",
      "isApproved": true,
      "images": [],
      "id": "0e19cb07-e2c7-453b-affd-f5bde5dc501d"
    }
  ]
}
```

2. Получить все отзывы по админке:
   * Доступ: Admin, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review/admin`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "title": "string",
      "content": "string",
      "isApproved": true,
      "images": [],
      "id": "03f4d37d-6940-4623-8195-dd47fd83ed57"
    },
    {
      "title": "string",
      "content": "string",
      "isApproved": true,
      "images": [
        {
          "imageUri": "string",
          "id": "c4e40610-9501-4a2f-a556-106aeb54edf3"
        }
      ],
      "id": "80c5950d-07bc-445f-8808-e850cd0d6989"
    }
  ]
}
```

3. Получить отзыв по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "Лучший отзыв на свете",
    "content": "ПРошли нахуй пидоры",
    "isApproved": true,
    "images": [],
    "id": "0e19cb07-e2c7-453b-affd-f5bde5dc501d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```

4. Получить все отзывы пользователя по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review/user/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "title": "string",
      "content": "string",
      "isApproved": true,
      "images": [],
      "id": "80c5950d-07bc-445f-8808-e850cd0d6989"
    },
    {
      "title": "Лучший отзыв на свете",
      "content": "ПРошли нахуй пидоры",
      "isApproved": true,
      "images": [],
      "id": "0e19cb07-e2c7-453b-affd-f5bde5dc501d"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
}
```

5. Получить все свои отзывы:
   * Доступ: All
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review/user`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "title": "string",
      "content": "string",
      "isApproved": true,
      "images": [],
      "id": "80c5950d-07bc-445f-8808-e850cd0d6989"
    },
    {
      "title": "Лучший отзыв на свете",
      "content": "ПРошли нахуй пидоры",
      "isApproved": true,
      "images": [],
      "id": "0e19cb07-e2c7-453b-affd-f5bde5dc501d"
    }
  ]
}
```

6. Получить все картинки отзывов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review/images`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "imageUri": "stringA",
      "id": "bf40c042-c78c-47c7-8681-09f98c17d848"
    },
    {
      "imageUri": "string",
      "id": "c4e40610-9501-4a2f-a556-106aeb54edf3"
    }
  ]
}
```

7. Получить все картинки отзыва:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review/{id}/images`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "imageUri": "stringA",
      "id": "bf40c042-c78c-47c7-8681-09f98c17d848"
    },
    {
      "imageUri": "string",
      "id": "c4e40610-9501-4a2f-a556-106aeb54edf3"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```

8. Получить картинку отзыва по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/user-review/image/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "imageUri": "stringA",
    "id": "bf40c042-c78c-47c7-8681-09f98c17d848"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "ReviewImage is not found. "
}
```

9. Создать отзыв:
   * Доступ: All
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/user-review`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "string",
  "content": "string",
  "isApproved": true,
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "Лучший отзыв на свете",
    "content": "ПРошли нахуй пидоры",
    "isApproved": true,
    "images": null,
    "id": "4a64e732-b432-4f90-9870-fcf8235db77a"
  }
}
```

10. Обновить отзыв:
    * Доступ: All
    * Метод: PUT
    * Запрос: `https://r.api.incase.com/api/user-review`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "80c5950d-07bc-445f-8808-e850cd0d6989",
  "title": "string",
  "content": "string",
  "isApproved": true,
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "string",
    "content": "string",
    "isApproved": true,
    "images": null,
    "id": "80c5950d-07bc-445f-8808-e850cd0d6989"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```

11. Обновить отзыв по админке:
    * Доступ: Admin, Owner, Bot
    * Метод: PUT
    * Запрос: `https://r.api.incase.com/api/user-review/admin`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "80c5950d-07bc-445f-8808-e850cd0d6989",
  "title": "string2131",
  "content": "string12312",
  "isApproved": true,
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "string2131",
    "content": "string12312",
    "isApproved": true,
    "images": null,
    "id": "80c5950d-07bc-445f-8808-e850cd0d6989"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```

12. Удалить отзыв по id:
    * Доступ: All
    * Метод: Delete
    * Запрос: `https://r.api.incase.com/api/user-review/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "Лучший отзыв на свете",
    "content": "ПРошли нахуй пидоры",
    "isApproved": true,
    "images": null,
    "id": "0e19cb07-e2c7-453b-affd-f5bde5dc501d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```

13. Удалить отзыв по админке и id:
    * Доступ: Admin, Owner, Bot
    * Метод: Delete
    * Запрос: `https://r.api.incase.com/api/user-review/admin/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "string",
    "content": "string",
    "isApproved": true,
    "images": null,
    "id": "03f4d37d-6940-4623-8195-dd47fd83ed57"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```

14. Удалить картинку отзыва по админке и id:
    * Доступ: Admin, Owner, Bot
    * Метод: Delete
    * Запрос: `https://r.api.incase.com/api/user-review/admin/image/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "imageUri": "string",
    "id": "c4e40610-9501-4a2f-a556-106aeb54edf3"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```

15. Удалить картинку отзыва по id картинки
    * Доступ: All
    * Метод: Delete
    * Запрос: `https://r.api.incase.com/api/user-review/admin/image/{imageId}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "imageUri": "stringA",
    "id": "bf40c042-c78c-47c7-8681-09f98c17d848"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "ReviewImage is not found. "
}
```

16. Создать картинку для отзыва:
    * Доступ: All
    * Метод: POST
    * Запрос: `https://r.api.incase.com/api/user-review`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "imageUri": "string",
  "reviewId": "80c5950d-07bc-445f-8808-e850cd0d6989"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "imageUri": "stringA",
    "id": "fc61e820-36be-4449-80a3-63028905a8b9"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
}
```
### Информация об обращениях в техническую поддержку

1. Получить все обращения авторизованного пользователя в техническую поддержку:
   * Доступ: User
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "title": "test topic",
      "content": "some content",
      "date": "2023-04-10T03:54:20.47",
      "isClosed": false,
      "answers": null,
      "id": "82aa8fc8-8cb3-44f1-b852-458a5ccbac38"
    },
    {
      "title": "test topic 2",
      "content": "some content 2",
      "date": "2023-04-10T03:55:27.6366667",
      "isClosed": false,
      "answers": null,
      "id": "e98c4b69-a7e3-4407-bbda-65475bcc262d"
    }
  ]
}
```

2. Получить определенное обращение авторизованного пользователя в техническую поддержку:
   * Доступ: User
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "test topic",
    "content": "some content",
    "date": "2023-04-10T03:54:20.47",
    "isClosed": false,
    "answers": null,
    "id": "82aa8fc8-8cb3-44f1-b852-458a5ccbac38"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopic is not found. "
}
```

3. Получить ответы на определенное обращение в техническую поддержку:
   * Доступ: User
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic/{id}/answers`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "content": "string",
      "date": "2023-04-09T23:01:42.748",
      "plaintiff": {
        "login": "zmqp",
        "email": "zxc.danil@inbox.ru",
        "passwordHash": "rYcUHdf7vdNMn4xKWN/htkx8k/8sKeJ3BEsg1HdNJGc=",
        "passwordSalt": "eLTy28AeXkKZu7shyy1VtaspWqUSLLTv0v3MezQ75f0FptyN0g7pzn1lBGKRwYIqTMYR495udcmy1hrSzgCesw==",
        "additionalInfo": null,
        "topics": null,
        "restrictions": null,
        "ownerRestrictions": null,
        "reviews": null,
        "historyPayments": null,
        "id": "555758dc-51a6-434f-bc3c-0543a005e28d"
      },
      "images": [],
      "id": "2ace9264-d8f7-468f-acfe-0bdb0edf305e"
    },
    {
      "content": "string",
      "date": "2023-04-09T23:01:42.748",
      "plaintiff": {
        "login": "zmqp",
        "email": "zxc.danil@inbox.ru",
        "passwordHash": "rYcUHdf7vdNMn4xKWN/htkx8k/8sKeJ3BEsg1HdNJGc=",
        "passwordSalt": "eLTy28AeXkKZu7shyy1VtaspWqUSLLTv0v3MezQ75f0FptyN0g7pzn1lBGKRwYIqTMYR495udcmy1hrSzgCesw==",
        "additionalInfo": null,
        "topics": null,
        "restrictions": null,
        "ownerRestrictions": null,
        "reviews": null,
        "historyPayments": null,
        "id": "555758dc-51a6-434f-bc3c-0543a005e28d"
      },
      "images": [],
      "id": "58682361-fe2c-43f8-acc5-9e12ce8e3e85"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopic is not found. "
}
```

4. Получить определенный ответ на определенное обращение в техническую поддержку:
   * Доступ: User
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic/{id}/answer/{answerId}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "content": "string",
    "date": "2023-04-09T23:01:42.748",
    "plaintiff": {
      "login": "zmqp",
      "email": "zxc.danil@inbox.ru",
      "passwordHash": "rYcUHdf7vdNMn4xKWN/htkx8k/8sKeJ3BEsg1HdNJGc=",
      "passwordSalt": "eLTy28AeXkKZu7shyy1VtaspWqUSLLTv0v3MezQ75f0FptyN0g7pzn1lBGKRwYIqTMYR495udcmy1hrSzgCesw==",
      "additionalInfo": null,
      "topics": null,
      "restrictions": null,
      "ownerRestrictions": null,
      "reviews": null,
      "historyPayments": null,
      "id": "555758dc-51a6-434f-bc3c-0543a005e28d"
    },
    "images": [],
    "id": "58682361-fe2c-43f8-acc5-9e12ce8e3e85"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

5. Получить все обращение в техническую поддержку:
   * Доступ: Support, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic/support`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "title": "test topic",
      "content": "some content",
      "date": "2023-04-10T03:54:20.47",
      "isClosed": false,
      "answers": null,
      "id": "82aa8fc8-8cb3-44f1-b852-458a5ccbac38"
    },
    {
      "title": "test topic 2",
      "content": "some content 2",
      "date": "2023-04-10T03:55:27.6366667",
      "isClosed": false,
      "answers": null,
      "id": "e98c4b69-a7e3-4407-bbda-65475bcc262d"
    },
    {
      "title": "добавим",
      "content": "соли",
      "date": "2023-04-09T22:57:12.14",
      "isClosed": true,
      "answers": null,
      "id": "a26750f8-f17a-4060-9acd-ca9074a630f7"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopic is not found. "
}
```

6. Получить определенный ответ на обращение в техническую поддержку:
   * Доступ: Support, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic/support/answer/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "content": "string",
    "date": "2023-04-09T23:01:42.748",
    "plaintiff": {
      "login": "zmqp",
      "email": "zxc.danil@inbox.ru",
      "passwordHash": "rYcUHdf7vdNMn4xKWN/htkx8k/8sKeJ3BEsg1HdNJGc=",
      "passwordSalt": "eLTy28AeXkKZu7shyy1VtaspWqUSLLTv0v3MezQ75f0FptyN0g7pzn1lBGKRwYIqTMYR495udcmy1hrSzgCesw==",
      "additionalInfo": null,
      "topics": null,
      "restrictions": null,
      "ownerRestrictions": null,
      "reviews": null,
      "historyPayments": null,
      "id": "555758dc-51a6-434f-bc3c-0543a005e28d"
    },
    "images": [],
    "id": "2ace9264-d8f7-468f-acfe-0bdb0edf305e"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

7. Получить все ответы на обращение в техническую поддержку:
   * Доступ: Support, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic/{id}/support/answers`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": [
    {
      "content": "string",
      "date": "2023-04-09T23:01:42.748",
      "plaintiff": {
        "login": "zmqp",
        "email": "zxc.danil@inbox.ru",
        "passwordHash": "rYcUHdf7vdNMn4xKWN/htkx8k/8sKeJ3BEsg1HdNJGc=",
        "passwordSalt": "eLTy28AeXkKZu7shyy1VtaspWqUSLLTv0v3MezQ75f0FptyN0g7pzn1lBGKRwYIqTMYR495udcmy1hrSzgCesw==",
        "additionalInfo": null,
        "topics": null,
        "restrictions": null,
        "ownerRestrictions": null,
        "reviews": null,
        "historyPayments": null,
        "id": "555758dc-51a6-434f-bc3c-0543a005e28d"
      },
      "images": [],
      "id": "2ace9264-d8f7-468f-acfe-0bdb0edf305e"
    }
  ]
}
```

8. Получить определенное обращение в техническую поддержку:
   * Доступ: Support, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/support-topic/{id}/support`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "test topic",
    "content": "some content",
    "date": "2023-04-10T03:54:20.47",
    "isClosed": false,
    "answers": null,
    "id": "82aa8fc8-8cb3-44f1-b852-458a5ccbac38"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopic is not found. "
}
```

9. Обратиться в техническую поддержку со стороны пользователя:
   * Доступ: User
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/support-topic`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "string",
  "content": "string",
  "date": "2023-04-09T23:39:12.314Z",
  "isClosed": true,
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "добавим",
    "content": "соли",
    "date": "2023-04-09T22:57:12.14Z",
    "isClosed": true,
    "answers": null,
    "id": "c19c9400-a6b6-4d6d-904c-813771c04e94"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Access denied"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "описание ошибки"
}
```

10. Ответ на вопрос в технической поддержке:
   * Доступ: Support, Owner, Bot, User
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/support-topic/answer`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "content": "string",
  "date": "2023-04-09T23:38:56.236Z",
  "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "content": "ответочка от админочка",
    "date": "2023-04-09T23:24:25.874Z",
    "plaintiff": null,
    "images": null,
    "id": "1564eebe-b240-4902-b963-535c235dc10d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopic is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "описание ошибки"
}
```

11. Изменение ответа на вопрос в технической поддержке со стороны администрации:
   * Доступ: Support, Owner, Bot, User
   * Метод: PUT
   * Запрос: `https://r.api.incase.com/api/support-topic`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "2D767AF3-6E4B-49D9-BF7B-BCA9E05EE7F1",
  "content": "string",
  "date": "2023-04-09T23:55:56.347Z",
  "plaintiffId": "555758DC-51A6-4342-B23C-0543A005228D",
  "topicId": "82AA8FC8-8CB3-44F1-B852-458A5CCBAC32"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "string",
    "content": "string",
    "date": "2023-04-09T23:29:19.714Z",
    "isClosed": true,
    "answers": null,
    "id": "82aa8fc8-8cb3-44f1-b852-458a5ccbac38"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

12. Изменение ответа на вопрос в технической поддержке со стороны пользователя:
   * Доступ: User
   * Метод: PUT
   * Запрос: `https://r.api.incase.com/api/support-topic/answer`

![](https://img.shields.io/static/v1?label=&message=Request_Body:&color=blue)
```JSON
{
  "id": "2D767AF3-6E4B-49D9-BF7B-BCA9E05EE7F1",
  "content": "string",
  "date": "2023-04-09T23:55:56.347Z",
  "plaintiffId": "555758DC-51A6-4342-B23C-0543A005228D",
  "topicId": "82AA8FC8-8CB3-44F1-B852-458A5CCBAC32"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "content": "string",
    "date": "2023-04-09T23:55:56.347Z",
    "plaintiff": null,
    "images": null,
    "id": "2d767af3-6e4b-49d9-bf7b-bca9e05ee7f1"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_409:&color=orange)
```JSON
{
  "success": false,
  "data": "Permission denied"
}
```

13. Удаление ответа на вопрос в технической поддержке со стороны пользователя:
   * Доступ: User
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/support-topic/answer/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": "SupportTopicAnswer"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

14. Удаление ответа на вопрос в технической поддержке со стороны администратора:
   * Доступ: Support, Owner, Bot
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/support-topic/support/answer/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": "SupportTopicAnswer"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

15. Удаление определенного топика в технической поддержке со стороны администратора:
   * Доступ: Owner, Bot
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/support-topic/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "success": true,
  "data": {
    "title": "test topic",
    "content": "some content",
    "date": "2023-04-10T03:54:20.47",
    "isClosed": false,
    "answers": null,
    "id": "82aa8fc8-8cb3-44f1-b852-458a5ccbac38"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

**В дальнейшем данное Api будет дорабатываться с обратной совместимостью, все нововведения будут расписаны**
