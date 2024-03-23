# Документация к базам данных

<img src="https://sun9-9.userapi.com/impg/TvxOs5Z6Oq4zIVtUnJD0uvbLUPHa86M0OkuSBQ/xwSvc-KOU-s.jpg?size=107x55&quality=96&sign=80e1a5000a20607c8bd1afe5453abefc&type=album" align="right"/>

#### [Наш сайт](https://in-case.games) | [API](https://api.in-case.games/api/) | [README](../README.md)

> Коммерческий проект по открытию кейсов.</br>
> Кейсы могут содержать в себе любые предметы из
> списка подключенных игр.

# === AnswerImage ===

Описание:

- Таблица представляет из себя коллекцию изображений для ответов

Связи:

- SupportTopicAnswer (Многие к одному)

| Key | Name     | Type             | IsRequired | Constrains      |
| :-- | :------- | :--------------- | :--------- | :-------------- |
| PK  | Id       | uniqueidentifier | True       | NEWID(), UNIQUE |
| FK  | AnswerId | uniqueidentifier | True       | -               |

**SupportTopicAnswer**

Описание:

- Таблица представляет из себя коллекцию ответов пользователя или тех поддержки, которая относится к обсуждению

Связи:

- User:Plaintiff (Многие к одному)
- SupportTopic (Многие к одному)

| Key | Name        | Type             | IsRequired | Constrains      |
| :-- | :---------- | :--------------- | :--------- | :-------------- |
| PK  | Id          | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Content     | nvarchar(120)    | True       | -               |
|     | Date        | datetime2(7)     | True       | -               |
| FK  | PlaintiffId | uniqueidentifier | False      | -               |
| FK  | TopicId     | uniqueidentifier | True       | -               |

**SupportTopic**

Описание:

- Таблица представляет из себя коллекцию обсуждений пользователя и поддержки

Связи:

- User(Многие к одному)

| Key | Name     | Type             | IsRequired | Constrains      |
| :-- | :------- | :--------------- | :--------- | :-------------- |
| PK  | Id       | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Title    | nvarchar(50)     | True       | -               |
|     | Content  | nvarchar(120)    | True       | -               |
|     | Date     | datetime2(7)     | True       | -               |
|     | IsClosed | bit              | True       | -               |
| FK  | UserId   | uniqueidentifier | True       | -               |

**UserAdditionalInfo**

Описание:

- Таблица представляет из себя дополнительную информацию пользователя

Связи:

- User(Один к одному)
- UserRole:Role(Один к одному)

| Key | Name          | Type             | IsRequired | Constrains      |
| :-- | :------------ | :--------------- | :--------- | :-------------- |
| PK  | Id            | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Balance       | decimal(18, 5)   | True       | -               |
|     | ImageUri      | nvarchar(MAX)    | True       | -               |
|     | IsNotifyEmail | bit              | True       | -               |
|     | IsGuestMode   | bit              | True       | -               |
|     | IsConfirmed   | bit              | True       | -               |
|     | CreationDate  | datetime2(7)     | True       | -               |
|     | DeletionDate  | datetime2(7)     | False      | -               |
| FK  | RoleId        | uniqueidentifier | True       | -               |
| FK  | UserId        | uniqueidentifier | True       | -               |

**UserRole**

Описание:

- Таблица представляет из себя пользовательские роли

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(15)     | True       | UNIQUE          |

**PromocodeType**

Описание:

- Таблица представляет из себя типы промокодов

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**Promocode**

Описание:

- Таблица представляет из себя промокод

Связи:

- PromocodeType:Type(Один к одному)

| Key | Name              | Type             | IsRequired | Constrains      |
| :-- | :---------------- | :--------------- | :--------- | :-------------- |
| PK  | Id                | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name              | nvarchar(50)     | True       | UNIQUE          |
|     | Discount          | decimal(5,5)     | True       | -               |
|     | NumberActivations | int              | True       | -               |
|     | ExpirationDate    | datetime2(7)     | True       | -               |
| FK  | TypeId            | uniqueidentifier | True       | -               |

**UserHistoryPromocode**

Описание:

- Таблица представляет из себя коллекцию историю применения промокода пользователя

Связи:

- User(Многие к одному)
- Promocode(Многие к одному)

| Key | Name        | Type             | IsRequired | Constrains      |
| :-- | :---------- | :--------------- | :--------- | :-------------- |
| PK  | Id          | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Date        | datetime2(7)     | True       | -               |
|     | IsActivated | bit              | True       | -               |
| FK  | UserId      | uniqueidentifier | True       | -               |
| FK  | PromocodeId | uniqueidentifier | True       | -               |

**UserRestriction**

Описание:

- Таблица представляет из себя коллекцию эффектов пользователя

Связи:

- User(Многие к одному)
- User:Owner(Многие к одному)
- RestrictionType:Type(Один к одному)

| Key | Name           | Type             | IsRequired | Constrains      |
| :-- | :------------- | :--------------- | :--------- | :-------------- |
| PK  | Id             | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | CreationDate   | datetime2(7)     | True       | -               |
|     | ExpirationDate | datetime2(7)     | True       | -               |
|     | Description    | nvarchar(120)    | False      | -               |
| FK  | UserId         | uniqueidentifier | True       | -               |
| FK  | OwnerId        | uniqueidentifier | False      | -               |
| FK  | TypeId         | uniqueidentifier | True       | -               |

**RestrictionType**

Описание:

- Таблица представляет из себя тип эффекта

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**ReviewImage**

Описание:

- Таблица представляет из себя коллекцию картинок для отзывов

Связи:

- UserReview:Review(Многие к одному)

| Key | Name     | Type             | IsRequired | Constrains      |
| :-- | :------- | :--------------- | :--------- | :-------------- |
| PK  | Id       | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | ImageUri | nvarchar(MAX)    | True       | -               |
| FK  | ReviewId | uniqueidentifier | True       | -               |

**UserReview**

Описание:

- Таблица представляет из себя коллекцию отзывов пользователя

Связи:

- User(Многие к одному)

| Key | Name       | Type             | IsRequired | Constrains      |
| :-- | :--------- | :--------------- | :--------- | :-------------- |
| PK  | Id         | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Title      | nvarchar(50)     | True       | -               |
|     | Content    | nvarchar(120)    | True       | -               |
|     | IsApproved | bit              | True       | -               |
| FK  | ReviewId   | uniqueidentifier | True       | -               |

**User**

Описание:

- Таблица представляет из себя пользователя

| Key | Name         | Type             | IsRequired | Constrains      |
| :-- | :----------- | :--------------- | :--------- | :-------------- |
| PK  | Id           | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Login        | nvarchar(50)     | True       | -               |
|     | Email        | nvarchar(50)     | True       | -               |
|     | PasswordSalt | nvarchar(MAX)    | True       | -               |
|     | PasswordHash | nvarchar(MAX)    | True       | -               |

**UserHistoryPayment**

Описание:

- Таблица представляет из себя коллекцию истории пополнения

Связи:

- User(Многие к одному)
- InvoicePaymentStatus:Status(Один к одному)

| Key | Name      | Type             | IsRequired | Constrains      |
| :-- | :-------- | :--------------- | :--------- | :-------------- |
| PK  | Id        | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | InvoiceId | nvarchar(MAX)    | True       | -               |
|     | Date      | datetime2(7)     | True       | -               |
|     | Currency  | nvarchar(MAX)    | True       | -               |
|     | Amount    | decimal(18, 5)   | True       | -               |
|     | Rate      | decimal(6, 5)    | True       | -               |
| FK  | StatusId  | uniqueidentifier | True       | -               |
| FK  | UserId    | uniqueidentifier | True       | -               |

**InvoicePaymentStatus**

Описание:

- Таблица представляет из себя коллекцию видов статусов оплаты

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**NewsImage**

Описание:

- Таблица представляет из себя коллекцию картинок для новостей

Связи:

- News(Многие к одному)

| Key | Name     | Type             | IsRequired | Constrains      |
| :-- | :------- | :--------------- | :--------- | :-------------- |
| PK  | Id       | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | ImageUri | nvarchar(MAX)    | True       | -               |
| FK  | NewsId   | uniqueidentifier | True       | -               |

**News**

Описание:

- Таблица представляет из себя новость

| Key | Name    | Type             | IsRequired | Constrains      |
| :-- | :------ | :--------------- | :--------- | :-------------- |
| PK  | Id      | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Title   | nvarchar(50)     | True       | -               |
|     | Date    | datetime2(7)     | True       | -               |
|     | Content | nvarchar(MAX)    | True       | -               |

**SiteStatisticsAdmin**

Описание:

- Таблица представляет из себя статистику сайта для админов

| Key | Name             | Type             | IsRequired | Constrains      |
| :-- | :--------------- | :--------------- | :--------- | :-------------- |
| PK  | Id               | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | BalanceWithdrawn | decimal(18, 5)   | True       | -               |
|     | TotalReplenished | decimal(18, 5)   | True       | -               |
|     | SentSites        | decimal(18, 5)   | True       | -               |

**SiteStatistics**

Описание:

- Таблица представляет из себя статистику сайта

| Key | Name           | Type             | IsRequired | Constrains      |
| :-- | :------------- | :--------------- | :--------- | :-------------- |
| PK  | Id             | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Users          | int              | True       | -               |
|     | Reviews        | int              | True       | -               |
|     | LootBoxes      | int              | True       | -               |
|     | WithdrawnItems | int              | True       | -               |
|     | WithdrawnFunds | int              | True       | -               |

**GroupLootBox**

Описание:

- Таблица представляет из себя группу кейсов

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**LootBoxGroup**

Описание:

- Таблица представляет из себя коллекцию группы кейсов

Связи:

- GroupLootBox:Group(Один к одному)
- Game(Многие к одному)
- LootBox:Box(Многие к одному)

| Key | Name    | Type             | IsRequired | Constrains      |
| :-- | :------ | :--------------- | :--------- | :-------------- |
| PK  | Id      | uniqueidentifier | True       | NEWID(), UNIQUE |
| FK  | BoxId   | uniqueidentifier | True       | -               |
| FK  | GroupId | uniqueidentifier | True       | -               |
| FK  | GameId  | uniqueidentifier | True       | -               |

**LootBoxBanner**

Описание:

- Таблица представляет из себя ивентовый баннер для кейса

Связи:

- LootBox:Box(Один к одному)

| Key | Name           | Type             | IsRequired | Constrains      |
| :-- | :------------- | :--------------- | :--------- | :-------------- |
| PK  | Id             | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | IsActive       | bit              | True       | -               |
|     | CreationDate   | datetime2(7)     | True       | -               |
|     | ImageUri       | nvarchar(MAX)    | True       | -               |
|     | ExpirationDate | datetime2(7)     | False      | -               |
| FK  | BoxId          | uniqueidentifier | True       | -               |

**LootBox**

Описание:

- Таблица представляет из себя кейс

Связи:

- Game(Многие к одному)

| Key | Name           | Type             | IsRequired | Constrains      |
| :-- | :------------- | :--------------- | :--------- | :-------------- |
| PK  | Id             | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name           | nvarchar(50)     | True       | -               |
|     | Cost           | decimal(18, 5)   | True       | -               |
|     | Balance        | decimal(18, 5)   | True       | -               |
|     | VirtualBalance | decimal(18, 5)   | True       | -               |
|     | ImageUri       | nvarchar(MAX)    | True       | -               |
|     | IsLocked       | bit              | True       | -               |
| FK  | GameId         | uniqueidentifier | True       | -               |

**Game**

Описание:

- Таблица представляет из себя игру

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**GameMarket**

Описание:

- Таблица представляет из себя коллекцию магазинов для игры

Связи:

- Game(Многие к одному)

| Key | Name   | Type             | IsRequired | Constrains      |
| :-- | :----- | :--------------- | :--------- | :-------------- |
| PK  | Id     | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name   | nvarchar(50)     | True       | UNIQUE          |
| FK  | GameId | uniqueidentifier | True       | -               |

**LootBoxInventory**

Описание:

- Таблица представляет из себя коллекцию инвентаря кейсов

Связи:

- LootBox:Box(Многие к одному)
- GameItem:Item(Многие к одному)

| Key | Name         | Type             | IsRequired | Constrains      |
| :-- | :----------- | :--------------- | :--------- | :-------------- |
| PK  | Id           | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | ChanceWining | int              | True       | -               |
| FK  | BoxId        | uniqueidentifier | True       | -               |
| FK  | ItemId       | uniqueidentifier | True       | -               |

**GameItem**

Описание:

- Таблица представляет из себя игровой предмет

Связи:

- Game(Многие к одному)
- GameItemType:Type(Один к одному)
- GameItemRarity:Rarity(Один к одному)
- GameItemQuality:Quality(Один к одному)

| Key | Name        | Type             | IsRequired | Constrains      |
| :-- | :---------- | :--------------- | :--------- | :-------------- |
| PK  | Id          | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name        | nvarchar(50)     | True       | -               |
|     | HashName    | nvarchar(MAX)    | True       | -               |
|     | Cost        | decimal(18, 5)   | True       | -               |
|     | ImageUri    | nvarchar(MAX)    | True       | -               |
|     | IdForMarket | nvarchar(MAX)    | False      | -               |
| FK  | GameId      | uniqueidentifier | True       | -               |
| FK  | TypeId      | uniqueidentifier | False      | -               |
| FK  | RarityId    | uniqueidentifier | False      | -               |
| FK  | QualityId   | uniqueidentifier | False      | -               |

**GameItemRarity**

Описание:

- Таблица представляет из себя редкость предмета

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**GameItemQuality**

Описание:

- Таблица представляет из себя качество предмета

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**GameItemType**

Описание:

- Таблица представляет из себя тип предмета

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**UserHistoryWithdraw**

Описание:

- Таблица представляет из себя коллекцию истории вывода

Связи:

- User(Многие к одному)
- GameItem:Item(Многие к одному)
- ItemWithdrawStatus:Status(Один к одному)
- GameMarket:Market(Многие к одному)

| Key | Name        | Type             | IsRequired | Constrains      |
| :-- | :---------- | :--------------- | :--------- | :-------------- |
| PK  | Id          | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | IdForMarket | int              | True       | UNIQUE          |
|     | Date        | datetime2(7)     | True       | -               |
|     | FixedCost   | decimal(18,5)    | True       | -               |
| FK  | UserId      | uniqueidentifier | True       | -               |
| FK  | ItemId      | uniqueidentifier | True       | -               |
| FK  | StatusId    | uniqueidentifier | True       | -               |
| FK  | MarketId    | uniqueidentifier | True       | -               |

**ItemWithdrawStatus**

Описание:

- Таблица представляет из себя статусы вывода предмета

| Key | Name | Type             | IsRequired | Constrains      |
| :-- | :--- | :--------------- | :--------- | :-------------- |
| PK  | Id   | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Name | nvarchar(50)     | True       | UNIQUE          |

**UserInventory**

Описание:

- Таблица представляет из себя коллекцию инвентаря пользователя

Связи:

- User(Многие к одному)
- GameItem:Item(Многие к одному)

| Key | Name      | Type             | IsRequired | Constrains      |
| :-- | :-------- | :--------------- | :--------- | :-------------- |
| PK  | Id        | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Date      | datetime2(7)     | True       | -               |
|     | FixedCost | decimal(18, 5)   | True       | -               |
| FK  | UserId    | uniqueidentifier | True       | -               |
| FK  | ItemId    | uniqueidentifier | True       | -               |

**UserHistoryOpening**

Описание:

- Таблица представляет из себя коллекцию истории открытия кейсов

Связи:

- User(Многие к одному)
- GameItem:Item(Многие к одному)
- LootBox:Box(Многие к одному)

| Key | Name   | Type             | IsRequired | Constrains      |
| :-- | :----- | :--------------- | :--------- | :-------------- |
| PK  | Id     | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Date   | datetime2(7)     | True       | -               |
| FK  | UserId | uniqueidentifier | True       | -               |
| FK  | BoxId  | uniqueidentifier | True       | -               |
| FK  | ItemId | uniqueidentifier | True       | -               |

**UserPathBanner**

Описание:

- Таблица представляет из себя коллекцию путей до ивентовых баннеров

Связи:

- User(Многие к одному)
- GameItem:Item(Многие к одному)
- LootBoxBanner:Banner(Многие к одному)

| Key | Name        | Type             | IsRequired | Constrains      |
| :-- | :---------- | :--------------- | :--------- | :-------------- |
| PK  | Id          | uniqueidentifier | True       | NEWID(), UNIQUE |
|     | Date        | datetime2(7)     | True       | -               |
|     | NumberSteps | int              | True       | -               |
|     | FixedCost   | decimal(18, 5)   | True       | -               |
| FK  | UserId      | uniqueidentifier | True       | -               |
| FK  | ItemId      | uniqueidentifier | True       | -               |
| FK  | BannerId    | uniqueidentifier | True       | -               |