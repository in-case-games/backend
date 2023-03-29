Документация к базе данных InCase.

**AnswerImage**

Описание:

- Таблица представляет из себя коллекцию изображений для ответов

Связи:

- SupportTopicAnswer (Многие к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Uri|nvarchar(MAX)|True|-|
|FK|AnswerId|uniqueidentifier|True|-|

**SupportTopicAnswer**

Описание:

- Таблица представляет из себя коллекцию ответов пользователя или тех поддержки, которая относится к обсуждению

Связи:

- User:Plaintiff (Многие к одному)
- SupportTopic (Многие к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Content|nvarchar(120)|True|-|
||Date|datetime2(7)|True|-|
|FK|PlaintiffId|uniqueidentifier|True|-|
|FK|TopicId|uniqueidentifier|True|-|

**SupportTopic**

Описание:

- Таблица представляет из себя коллекцию обсуждений пользователя и поддержки

Связи:

- User(Многие к одному)
- User:Support(Многие к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Title|nvarchar(50)|True|-|
||Content|nvarchar(120)|True|-|
||Date|datetime2(7)|True|-|
||IsClosed|bit|True|-|
|FK|UserId|uniqueidentifier|True|-|
|FK|SupportId|uniqueidentifier|False|-|

**UserToken**

Описание:

- Таблица представляет из себя коллекцию сессий пользователя

Связи:

- User(Многие к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Refresh|nvarchar(64)|True|-|
||Email|nvarchar(64)|True|-|
||IpAddress|nvarchar(15)|False|-|
||Device|nvarchar(15)|False|-|
|FK|UserId|uniqueidentifier|True|-|

**UserAdditionalInfo**

Описание:

- Таблица представляет из себя дополнительную информацию пользователя

Связи:

- User(Один к одному)
- UserRole:Role(Один к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Balance|decimal(18, 5)|True|-|
||Uri|nvarchar(MAX)|True|-|
||IsNotifyEmail|bit|True|-|
||IsGuestMode|bit|True|-|
|FK|RoleId|uniqueidentifier|True|-|
|FK|UserId|uniqueidentifier|True|-|


**UserRole**

Описание:

- Таблица представляет из себя пользовательские роли


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Name|nvarchar(15)|True|UNIQUE|

**PromocodeType**

Описание:

- Таблица представляет из себя типы промокодов


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Name|nvarchar(50)|True|UNIQUE|

**Promocode**

Описание:

- Таблица представляет из себя промокод

Связи:

- PromocodeType:Type(Один к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Name|nvarchar(50)|True|UNIQUE|
||Discount|int|True|-|
||NumberActivations|int|True|-|
||ExpirationDate|datetime2(7)|True|-|
|FK|TypeId|uniqueidentifier|True|-|

**UserHistoryPromocode**

Описание:

- Таблица представляет из себя коллекцию историю применения промокода пользователя

Связи:

- User(Многие к одному)
- Promocode(Многие к одному)



|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||ExpirationDate|datetime2(7)|True|-|
||IsActivated|bit|True|-|
|FK|UserId|uniqueidentifier|True|-|
|FK|PromocodeId|uniqueidentifier|True|-|
**UserRestriction**

Описание:

- Таблица представляет из себя коллекцию эффектов пользователя

Связи:

- User(Многие к одному)
- User:Owner(Многие к одному)
- RestrictionType:Type(Один к одному)



|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||CreationDate|datetime2(7)|True|-|
||ExpirationDate|datetime2(7)|True|-|
||Description|nvarchar(120)|False|-|
|FK|UserId|uniqueidentifier|True|-|
|FK|OwnerId|uniqueidentifier|True|-|
|FK|TypeId|uniqueidentifier|True|-|

**RestrictionType**

Описание:

- Таблица представляет из себя тип эффекта



|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Name|nvarchar(50)|True|UNIQUE|

**ReviewImage**

Описание:

- Таблица представляет из себя коллекцию картинок для отзывов

Связи:

- UserReview:Review(Многие к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Uri|nvarchar(MAX)|True|-|
|FK|ReviewId|uniqueidentifier|True|-|

**UserReview**

Описание:

- Таблица представляет из себя коллекцию отзывов пользователя

Связи:

- User(Многие к одному)


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Title|nvarchar(50)|True|-|
||Content|nvarchar(120)|True|-|
||IsApproved|bit|True|-|
|FK|ReviewId|uniqueidentifier|True|-|

**User**

Описание:

- Таблица представляет из себя пользователя


|Key|Name|Type|IsRequired|Constrains|
| :- | :- | :- | :- | :- |
|PK|Id|uniqueidentifier|True|NEWID(), UNIQUE|
||Login|nvarchar(50)|True|-|
||Email|nvarchar(50)|True|-|
||PasswordSalt|nvarchar(64)|True|-|
||PasswordHash|nvarchar(64)|True|-|

