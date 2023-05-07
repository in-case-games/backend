# Документация InCase.Resources.Api

**Содержание**

* Общее
  * **<a href="https://github.com/InCase-buy-and-opening-cases/InCase_backend/blob/InCase.dev/ApiResDocs.md#информация-об-играх">Информация об играх</a>**
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

В api есть самописные возраты кодов:
1. ![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
   * 0 - Bad Request
   * 1 - Unauthorized
   * 2 - Payment Required (Недостаточный баланс)
   * 3 - Forbidden (Доступ запрещен)
   * 4 - Not Found (Не найдено запись в бд)
   * 5 - Conflict (Конфликт возможно запись уже есть)
   * 6 - Request Timeout (Превышено время ожидания)
   * 7 - Unknown Error (Ошибку поймал try/catch)
2. ![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
   * 0 - OK
   * 1 - Accepted (Принято в обработку)
   * 2 - Sent Email (Отправлено на почту)


### Информация об играх

1. Получение всех игровых сущностей:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "name": "csgo",
      "items": [],
      "boxes": [],
      "markets": null,
      "id": "6ebf72d4-fb09-4716-b08e-57cd941da414"
    },
    {
      "name": "genshin",
      "items": [],
      "boxes": [],
      "markets": null,
      "id": "2a190be9-ceab-4252-a7a7-d305139c8af0"
    },
    {
      "name": "dota2",
      "items": [],
      "boxes": [],
      "markets": null,
      "id": "2fcd4179-6632-4ed3-9747-fa94656ffc5d"
    }
  ]
}
```

2. Получение определенной игровой сущности: 
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "csgo",
    "items": [],
    "boxes": [],
    "markets": null,
    "id": "6ebf72d4-fb09-4716-b08e-57cd941da414"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Игра не найдена"
  }
}
```

### Информация о статистике

1. Получение обычной статистике сайта:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/site-statistics`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "users": 0,
    "reviews": 0,
    "lootBoxes": 0,
    "withdrawnItems": 0,
    "withdrawnFunds": 0,
    "id": "49feea03-c827-40d2-856b-46bb8bbe9793"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Статистика не найдена"
  }
}
```

2. Получение админской статистике сайта:
   * Доступ: Owner, Bot
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/site-statistics/admin`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "balanceWithdrawn": 0,
    "totalReplenished": 0,
    "sentSites": 0,
    "id": "6842ae9b-f7d9-452e-b70a-31bbe7387ff4"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "success": false,
  "data": "Админская сайт статистика не найдена"
}
```

## Предметы

### Информация о предметах

1. Получение всех предметов:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "name": "типо имя предмета",
      "hashName": "хэш имя на официальной тп",
      "cost": 110,
      "imageUri": "string",
      "idForMarket": "string",
      "quality": {
        "name": "battle scarred",
        "id": "c752b723-d6d5-4370-8392-b10010e281d5"
      },
      "type": {
        "name": "gloves",
        "id": "dacfcecd-68a6-45da-a170-5ffc5f6e5b3b"
      },
      "rarity": {
        "name": "blue",
        "id": "c0503650-1ac9-478a-b5ed-265f94250de4"
      },
      "id": "82bd82ec-5c01-4fcf-af97-70d04d7768d2"
    }
  ]
}
```

2. Получение одного предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "типо имя предмета",
    "hashName": "хэш имя на официальной тп",
    "cost": 110,
    "imageUri": "string",
    "idForMarket": "string",
    "quality": {
      "name": "battle scarred",
      "id": "c752b723-d6d5-4370-8392-b10010e281d5"
    },
    "type": {
      "name": "gloves",
      "id": "dacfcecd-68a6-45da-a170-5ffc5f6e5b3b"
    },
    "rarity": {
      "name": "blue",
      "id": "c0503650-1ac9-478a-b5ed-265f94250de4"
    },
    "id": "82bd82ec-5c01-4fcf-af97-70d04d7768d2"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Предмет не найден"
  }
}
```

3. Получение всех качеств предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/qualities`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "name": "battle scarred",
      "id": "c752b723-d6d5-4370-8392-b10010e281d5"
    },
    {
      "name": "factory new",
      "id": "71925c1c-866c-4217-9666-d1ede06e67d6"
    },
    {
      "name": "field tested",
      "id": "e77a70f6-f4ba-4973-9334-fb6decf45112"
    },
    {
      "name": "minimal wear",
      "id": "25a74cf3-3447-45ae-ba60-61363c75698f"
    },
    {
      "name": "none",
      "id": "b3f62422-12f9-40b1-a818-ff3d0954e269"
    },
    {
      "name": "well worn",
      "id": "99efc9b5-058f-4910-b2c8-2f7c2476282e"
    }
  ]
}
```

4. Получение всех типов предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/types`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "name": "gloves",
      "id": "dacfcecd-68a6-45da-a170-5ffc5f6e5b3b"
    },
    {
      "name": "knife",
      "id": "126611fd-db44-4626-b0f1-2a472bd97d1d"
    },
    {
      "name": "none",
      "id": "82a1579f-66d1-423a-aea0-469920c34bfc"
    },
    {
      "name": "other",
      "id": "fb9d792b-3a76-425a-8d21-c3abd875e63f"
    },
    {
      "name": "pistol",
      "id": "546e6bcd-ef8f-48ae-abec-8cfc10725206"
    },
    {
      "name": "rifle",
      "id": "e9a19fb3-e113-4e99-98cd-e1754e6aeec8"
    },
    {
      "name": "weapon",
      "id": "c0851265-2702-4b64-ae9a-c4ddab77021e"
    }
  ]
}
```

5. Получение всех редкостей предмета:
   * Доступ: Allow Anonymous
   * Метод: GET 
   * Запрос: `https://r.api.incase.com/api/game-item/rarities`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "name": "blue",
      "id": "c0503650-1ac9-478a-b5ed-265f94250de4"
    },
    {
      "name": "gold",
      "id": "28806de3-b5f1-430b-af7c-0fbc5fb8d646"
    },
    {
      "name": "pink",
      "id": "86388a6e-6c7c-4494-8b85-2ee843e6eea0"
    },
    {
      "name": "red",
      "id": "2c7e92a8-3e12-4107-8035-2f40d77c494c"
    },
    {
      "name": "violet",
      "id": "ecdf73f2-a2a2-4be0-8bcb-e04ed4aef1e0"
    },
    {
      "name": "white",
      "id": "81a7a225-eb56-49cd-942f-5677dd061574"
    }
  ]
}
```

6. Создание нового предмета:
   * Доступ: Owner
   * Метод: POST 
   * Запрос: `https://r.api.incase.com/api/game-item`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "типо имя предмета",
  "hashName": "хэш имя на официальной тп",
  "cost": 110,
  "imageUri": "string",
  "idForMarket": "string",
  "gameId": "6ebf72d4-fb09-4716-b08e-57cd941da414",
  "typeId": "dacfcecd-68a6-45da-a170-5ffc5f6e5b3b",
  "rarityId": "c0503650-1ac9-478a-b5ed-265f94250de4",
  "qualityId": "c752b723-d6d5-4370-8392-b10010e281d5"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "типо имя предмета",
    "hashName": "хэш имя на официальной тп",
    "cost": 110,
    "imageUri": "string",
    "idForMarket": "string",
    "quality": null,
    "type": null,
    "rarity": null,
    "id": "82bd82ec-5c01-4fcf-af97-70d04d7768d2"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Игра не найдена"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Тип предмета не найден"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Редкость предмета не найдена"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Качество предмета не найдено"
  }
}
```

7. Обновление предмета:
   * Доступ: Owner
   * Метод: PUT 
   * Запрос: `https://r.api.incase.com/api/game-item`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "82bd82ec-5c01-4fcf-af97-70d04d7768d2",
  "name": "типо имя предмета1",
  "hashName": "хэш имя на официальной тп1",
  "cost": 1101,
  "imageUri": "string1",
  "idForMarket": "string1",
  "gameId": "6ebf72d4-fb09-4716-b08e-57cd941da414",
  "typeId": "dacfcecd-68a6-45da-a170-5ffc5f6e5b3b",
  "rarityId": "c0503650-1ac9-478a-b5ed-265f94250de4",
  "qualityId": "c752b723-d6d5-4370-8392-b10010e281d5"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "типо имя предмета1",
    "hashName": "хэш имя на официальной тп1",
    "cost": 1101,
    "imageUri": "string1",
    "idForMarket": "string1",
    "quality": null,
    "type": null,
    "rarity": null,
    "id": "82bd82ec-5c01-4fcf-af97-70d04d7768d2"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Запись таблицы GameItem по 81bd82ec-5c01-4fcf-af97-70d04d7768d2 не найдена"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Игра не найдена"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Тип предмета не найден"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Редкость предмета не найдена"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Качество предмета не найдено"
  }
}
```

8. Удаление предмета:
   * Доступ: Owner
   * Метод: Delete
   * Запрос: `https://r.api.incase.com/api/game-item/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "типо имя предмета1",
    "hashName": "хэш имя на официальной тп1",
    "cost": 1101,
    "imageUri": "string1",
    "idForMarket": "string1",
    "quality": null,
    "type": null,
    "rarity": null,
    "id": "82bd82ec-5c01-4fcf-af97-70d04d7768d2"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Запись таблицы GameItem по 81bd82ec-5c01-4fcf-af97-70d04d7768d2 не найдена"
  }
}
```


### Информация о группах кейсов

1. Получение всех групп кейсов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "group": {
        "name": "имя группы",
        "id": "637137cf-f67c-448a-9927-9271d4432334"
      },
      "box": {
        "name": "имя кейса",
        "cost": 111,
        "balance": 0,
        "virtualBalance": 0,
        "imageUri": "string",
        "isLocked": true,
        "inventories": null,
        "id": "91cd8256-575c-4367-ab63-a315364c024d"
      },
      "id": "6e86b05f-75a1-4a92-ad5f-6a8dd22d6051"
    }
  ]
}
```

2. Получение группы кейса по id:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "group": {
      "name": "имя группы",
      "id": "637137cf-f67c-448a-9927-9271d4432334"
    },
    "box": {
      "name": "имя кейса",
      "cost": 111,
      "balance": 0,
      "virtualBalance": 0,
      "imageUri": "string",
      "isLocked": true,
      "inventories": null,
      "id": "91cd8256-575c-4367-ab63-a315364c024d"
    },
    "id": "6e86b05f-75a1-4a92-ad5f-6a8dd22d6051"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс группа не найдена"
  }
}
```

3. Получение всех групп:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/groups`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "name": "имя группы",
      "id": "637137cf-f67c-448a-9927-9271d4432334"
    }
  ]
}
```

4. Получить все группы по id игры:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box-group/game/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "group": {
        "name": "имя группы",
        "id": "637137cf-f67c-448a-9927-9271d4432334"
      },
      "box": {
        "name": "имя кейса",
        "cost": 111,
        "balance": 0,
        "virtualBalance": 0,
        "imageUri": "string",
        "isLocked": true,
        "inventories": null,
        "id": "91cd8256-575c-4367-ab63-a315364c024d"
      },
      "id": "6e86b05f-75a1-4a92-ad5f-6a8dd22d6051"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Игра не найдена"
  }
}
```

5. Создание группы кейсов:
   * Доступ: Owner
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box-group`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "boxId": "91cd8256-575c-4367-ab63-a315364c024d",
  "groupId": "637137cf-f67c-448a-9927-9271d4432334",
  "gameId": "6ebf72d4-fb09-4716-b08e-57cd941da414"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "group": null,
    "box": null,
    "id": "6e86b05f-75a1-4a92-ad5f-6a8dd22d6051"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_404:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_404:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Группа кейсов не найдена"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_404:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Игра не найдена"
  }
}
```

6. Создание группы:
   * Доступ: Owner
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box-group/group`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "имя группы"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "имя группы",
    "id": "637137cf-f67c-448a-9927-9271d4432334"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_400:&color=red)
```JSON
{
  "error": {
    "code": "5",
    "message": "Имя группы уже используется"
  }
}
```


7. Удаление группы кейсов:
   * Доступ: Owner
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/loot-box-group/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "group": null,
    "box": null,
    "id": "6f32dc64-b95c-49f6-ab2e-8fb75d02c84c"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Запись таблицы LootBoxGroup по 6f32dc64-b95c-49f6-ab2e-8fb75d02c84c не найдена"
  }
}
```

8. Удаление группы:
   * Доступ: Owner
   * Метод: DELETE
   * Запрос: `https://r.api.incase.com/api/loot-box-group/group/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "имя группы",
    "id": "637137cf-f67c-448a-9927-9271d4432334"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Запись таблицы GroupLootBox по 637137cf-f67c-448a-9927-9271d4432334 не найдена"
  }
}
```

### Информация о кейсе

1. Получение всех кейсов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "name": "имя кейса",
      "cost": 111,
      "imageUri": "string",
      "isLocked": true,
      "gameId": "6ebf72d4-fb09-4716-b08e-57cd941da414",
      "id": "91cd8256-575c-4367-ab63-a315364c024d"
    }
  ]
}
```

2. Получение одного кейса:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "имя кейса",
    "cost": 111,
    "imageUri": "string",
    "isLocked": true,
    "gameId": "6ebf72d4-fb09-4716-b08e-57cd941da414",
    "id": "91cd8256-575c-4367-ab63-a315364c024d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
}
```

3. Получение содержимого кейса:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/{id}/inventory`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "chanceWining": 11,
      "item": {
        "name": "типо имя предмета",
        "hashName": "имя предмета на официальной тп",
        "cost": 110,
        "imageUri": "string",
        "idForMarket": "string",
        "quality": {
          "name": "battle scarred",
          "id": "c752b723-d6d5-4370-8392-b10010e281d5"
        },
        "type": {
          "name": "gloves",
          "id": "dacfcecd-68a6-45da-a170-5ffc5f6e5b3b"
        },
        "rarity": {
          "name": "blue",
          "id": "c0503650-1ac9-478a-b5ed-265f94250de4"
        },
        "id": "474fe485-4b63-47cb-8bcd-b681cd4530c9"
      },
      "id": "d0e2358c-a976-4b3f-a81d-a375eaee8671"
    }
  ]
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
}
```

4. Получение всех баннеров кейсов:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/banners`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": [
    {
      "isActive": true,
      "creationDate": "2023-05-07T14:11:29.787",
      "expirationDate": "2023-05-07T14:11:29.787",
      "imageUri": "string",
      "boxId": "91cd8256-575c-4367-ab63-a315364c024d",
      "id": "4e73a802-3a6a-4324-bce5-d9a7148b0ea7"
    }
  ]
}
```

5. Получение баннера у кейса:
   * Доступ: Allow Anonymous
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/{id}/banner`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "isActive": true,
    "creationDate": "2023-05-07T14:11:29.787",
    "expirationDate": "2023-05-07T14:11:29.787",
    "imageUri": "string",
    "boxId": "91cd8256-575c-4367-ab63-a315364c024d",
    "id": "4e73a802-3a6a-4324-bce5-d9a7148b0ea7"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Баннер не найден"
  }
}
```

6. Создать новый кейс:
   * Доступ: Owner
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "f7f2aa2c-ff68-4f54-961b-b86b6d04e9ba",
  "name": "имя кейса",
  "cost": 111,
  "imageUri": "string",
  "isLocked": true,
  "gameId": "6ebf72d4-fb09-4716-b08e-57cd941da414"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "имя кейса",
    "cost": 111,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string",
    "isLocked": true,
    "inventories": null,
    "id": "91cd8256-575c-4367-ab63-a315364c024d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Игра не найдена"
  }
}
```

7. Обновить кейс:
   * Доступ: Owner
   * Метод: PUT
   * Запрос: `https://r.api.incase.com/api/loot-box`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "name": "имя кейса1",
  "cost": 111,
  "imageUri": "string1",
  "isLocked": false,
  "gameId": "6ebf72d4-fb09-4716-b08e-57cd941da414",
  "id": "91cd8256-575c-4367-ab63-a315364c024d"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "имя кейса1",
    "cost": 111,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string1",
    "isLocked": false,
    "inventories": null,
    "id": "91cd8256-575c-4367-ab63-a315364c024d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Игра не найдена"
  }
}
```

8. Создать новое содержимое кейсу:
   * Доступ: Owner
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box/inventory`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "chanceWining": 11,
  "itemId": "474fe485-4b63-47cb-8bcd-b681cd4530c9",
  "boxId": "91cd8256-575c-4367-ab63-a315364c024d"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "chanceWining": 11,
    "item": null,
    "id": "cc8b9c25-0533-4169-890e-b8786715a5f0"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Предмет не найден"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
}
```

9. Создать баннер кейсу:
   * Доступ: Owner
   * Метод: POST
   * Запрос: `https://r.api.incase.com/api/loot-box/banner`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "isActive": true,
  "creationDate": "2023-05-07T14:31:34.915Z",
  "expirationDate": "2023-05-07T14:31:34.915Z",
  "imageUri": "string",
  "boxId": "91cd8256-575c-4367-ab63-a315364c024d"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "isActive": true,
    "creationDate": "2023-05-07T14:31:34.915Z",
    "expirationDate": "2023-05-07T14:31:34.915Z",
    "imageUri": "string",
    "box": null,
    "id": "9b5ad520-5f5e-41f7-bbda-67864b697a1e"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=orange)
```JSON
{
  "error": {
    "code": "5",
    "message": "Кейс уже использует баннер"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
}
```

10. Обновить баннер кейса:
    * Доступ: Owner
    * Метод: PUT
    * Запрос: `https://r.api.incase.com/api/loot-box/banner`

![](https://img.shields.io/static/v1?label=&message=REQUEST_BODY:&color=blue)
```JSON
{
  "id": "9b5ad520-5f5e-41f7-bbda-67864b697a1e",
  "isActive": false,
  "creationDate": "2023-06-07T14:34:57.585Z",
  "expirationDate": "2023-06-07T14:34:57.585Z",
  "imageUri": "string1",
  "boxId": "91cd8256-575c-4367-ab63-a315364c024d"
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "isActive": false,
    "creationDate": "2023-06-07T14:34:57.585Z",
    "expirationDate": "2023-06-07T14:34:57.585Z",
    "imageUri": "string1",
    "box": null,
    "id": "9b5ad520-5f5e-41f7-bbda-67864b697a1e"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "5",
    "message": "Баннер уже использует кейс"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Баннер не найден"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
}
```

11. Удалить кейс:
    * Доступ: Owner
    * Метод: DELETE
    * Запрос: `https://r.api.incase.com/api/loot-box/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "имя кейса1",
    "cost": 111,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string1",
    "isLocked": false,
    "inventories": null,
    "id": "91cd8256-575c-4367-ab63-a315364c024d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_404:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Запись таблицы LootBox по 91cd8256-575c-4367-ab63-a315364c024d не найдена"
  }
}
```

12. Удалить баннер кейса:
    * Доступ: Owner
    * Метод: DELETE
    * Запрос: `https://r.api.incase.com/api/loot-box/banner/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "isActive": true,
    "creationDate": "2023-05-07T14:11:29.787",
    "expirationDate": "2023-05-07T14:11:29.787",
    "imageUri": "string",
    "box": null,
    "id": "4e73a802-3a6a-4324-bce5-d9a7148b0ea7"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_404:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Запись таблицы LootBoxBanner по 4e73a802-3a6a-4324-bce5-d9a7148b0ea7 не найдена"
  }
}
```

13. Удалить содержимое кейса:
    * Доступ: Owner
    * Метод: DELETE
    * Запрос: `https://r.api.incase.com/api/loot-box/inventory/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "chanceWining": 11,
    "item": null,
    "id": "d0e2358c-a976-4b3f-a81d-a375eaee8671"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Запись таблицы LootBoxInventory по d0e2358c-a976-4b3f-a81d-a375eaee8671 не найдена"
  }
}
```

14. Получение одного кейса по админке:
   * Доступ: Admin, Owner, Bot
   * Метод: GET
   * Запрос: `https://r.api.incase.com/api/loot-box/admin/{id}`

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_200:&color=green)
```JSON
{
  "code": "0",
  "data": {
    "name": "имя кейса1",
    "cost": 111,
    "balance": 0,
    "virtualBalance": 0,
    "imageUri": "string1",
    "isLocked": false,
    "inventories": null,
    "id": "91cd8256-575c-4367-ab63-a315364c024d"
  }
}
```

![](https://img.shields.io/static/v1?label=&message=STATUS_CODE_400:&color=red)
```JSON
{
  "error": {
    "code": "4",
    "message": "Кейс не найден"
  }
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Promocode is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "Promocode is not found. "
}
```

5. Создать промокод:
   * Доступ: Owner
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
   * Доступ: Owner
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
   * Доступ: Admin, Owner
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserAdditionalInfo is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRole is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "User is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserInventory is not found. "
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
  "data": "UserInventory is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserPathBanner is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserHistoryPayment is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserHistoryOpening is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserHistoryWithdrawn is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserHistoryOpening is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserHistoryPromocodes is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserRestriction is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
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
  "data": "UserReview is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReview is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "UserReviewImage is not found. "
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
  "data": "UserReviewImage is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopic is not found. "
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
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
   * Доступ: Admin, Owner, Bot
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
   * Доступ: Admin, Owner, Bot
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
   * Доступ: Admin, Owner, Bot
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

![](https://img.shields.io/static/v1?label=&message=STATUS_MESSAGE_404:&color=red)
```JSON
{
  "success": false,
  "data": "SupportTopicAnswer is not found. "
}
```

8. Получить определенное обращение в техническую поддержку:
   * Доступ: Admin, Owner, Bot
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
   * Доступ: Admin, Owner
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

11. Изменение обсуждения:
   * Доступ: User, Admin, Owner
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

12. Изменение ответа:
   * Доступ: User, Admin, Owner
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

13. Удаление сообщения пользователем:
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

14. Удаление сообщения тех поддержкой:
   * Доступ: Admin, Owner
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

16. Отправка сообщения технической поддержке на вопрос:
    * Доступ: User
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

**В дальнейшем данное Api будет дорабатываться с обратной совместимостью, все нововведения будут расписаны**
