<!-- General -->

BASE_URI = https://localhost:5000
HTTP_SECURE = true
<!-- Generic Response -->

### 500 ResponseBody - {
    "error": {
        "code": 500,
        "message": "error message"
    }
}

# ===Authentication Service=== 
<!-- [C] Confirm -->
<!-- [M] ConfirmAccount -->
## GET - /api/authentication/confirm/account
Roles - Any

### RequestBody - { 
    "params": {
        "token": "string",
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "accessToken": "string",
        "refreshToken": "string",
        "expiresAccess": "2023-07-28T10:36:51.572Z",
        "expiresRefresh": "2023-07-28T10:36:51.572Z"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 0,
        "message": "error message"
    }
}
<!-- [M] UpdateEmail -->

## GET - /api/authentication/confirm/email/{email}
Roles - Any

### RequestBody - { 
    "queries": {
        "email": "string",
    },
    "params": {
        "token": "string",
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "string",
        "login": "string"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 0,
        "message": "error message"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 5,
        "message": "Email почта занята"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Не валидный {type} токен"
    }
}
<!-- [M] UpdateLogin -->

## GET - /api/authentication/confirm/login/{login}
Roles - Any

### RequestBody - { 
    "queries": {
        "email": "string",
    },
    "params": {
        "token": "string",
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "string",
        "login": "string"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 0,
        "message": "error message"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 5,
        "message": "Логин занят"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Не валидный {type} токен"
    }
}
<!-- [M] UpdatePassword -->

## GET - /api/authentication/confirm/password/{password}
Roles - Any

### RequestBody - { 
    "queries": {
        "password": "string",
    },
    "params": {
        "token": "string",
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "string",
        "login": "string"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 0,
        "message": "Пароль некорректный"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Не валидный {type} токен"
    }
}
<!-- [M] Delete -->

## DELETE - /api/authentication/confirm/account
Roles - Any

### RequestBody - { 
    "params": {
        "token": "string",
    }
}


### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "email": "string",
        "login": "string"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Не валидный {type} токен"
    }
}
<!-- [C] Authentication -->
<!-- [M] SignIn -->

## POST - /api/authentication/sign-in
Roles - Any

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "login": "string",
    "email": "string",
    "password": "string"
}

### 200 ResponseBody - {
    "code": 0,
    "data": "string"
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Неверный пароль"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вход запрещён до {ban.ExpirationDate}."
    }
}
<!-- [M] SignUp -->

## POST - /api/authentication/sign-up
Roles - Any

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "login": "string",
    "email": "string",
    "password": "string"
}

### 200 ResponseBody - {
    "code": 0,
    "data": "string"
}

### 400 ResponseBody - {
    "error": {
        "code": 5,
        "message": "Пользователь уже существует"
    }
}

<!-- [M] Refresh -->

## GET - /api/authentication/refresh
Roles - Any 

### RequestBody - {
    "params": {
        "token": "string"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "accessToken": "string",
        "refreshToken": "string",
        "expiresAccess": "2023-07-28T11:47:53.384Z",
        "expiresRefresh": "2023-07-28T11:47:53.384Z"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Не валидный {type} токен"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Аккаунт в очереди на удаление, отмените входом в аккаунт"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вход запрещён до {ban.ExpirationDate}."
    }
}
<!-- [C] AuthenticationSending -->
<!-- [M] ForgotPassword -->

## PUT - /api/authentication/sending/forgot/password
Roles - Any

### RequestBody - {
    "login": "string",
    "email": "string",
    "token": "string"
}

### 200 ResponseBody - {
    "code": 0,
    "data": "string"
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [M] UpdateEmail -->

## PUT - /api/authentication/sending/email/{password}

### RequestBody - {
    "queries": {
        "password": "string"
    },
    {
        "login": "string",
        "email": "string",
        "token": "string"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": "string"
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Неверный пароль"
    }
}
<!-- [M] UpdateLogin -->

## PUT - /api/authentication/sending/login/{login}

### RequestBody - {
    "queries": {
        "password": "string"
    },
    {
        "login": "string",
        "email": "string",
        "token": "string"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": "string"
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Неверный пароль"
    }
}
<!-- [M] UpdatePassword -->

## PUT - /api/authentication/sending/password/{password}

### RequestBody - {
    "queries": {
        "password": "string"
    },
    {
        "login": "string",
        "email": "string",
        "token": "string"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": "string"
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Неверный пароль"
    }
}
<!-- [M] DeleteAccount -->

## DELETE - /api/authentication/sending/confirm/{password}

### RequestBody - {
    "queries": {
        "password": "string"
    },
    {
        "login": "string",
        "email": "string",
        "token": "string"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": "string"
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Неверный пароль"
    }
}
# ===EmailSender Service=== 
<!-- [C] User -->
<!-- [M] GetByUserId -->

## GET - /api/user/{id}/is-notify
Roles - Any

### RequestBody {
    "queries": "GUID"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "isNotifyEmail": true,
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [M] Get -->

## GET - /api/user/is-notify
Roles - User, Admin , Owner , Bot

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "isNotifyEmail": true,
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [M] ChangeNotifyEmail -->

## GET - /api/user/is-notify/{isNotify}
Roles - User, Admin , Owner , Bot

### RequestBody - {
    "queries": {
        "isNotify": boolean
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "isNotifyEmail": true,
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [M] ChangeNotifyEmailByAdmin -->

## GET - /api/user/is-notify/{isNotify}
Roles - Admin , Owner

### RequestBody - {
    "queries": {
        "userId": "GUID",
        "isNotify": boolean
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "isNotifyEmail": true,
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
# ===Identity Service=== 
<!-- [C] User -->
<!-- [M] Get -->

## GET - /api/user/{id}
Roles - Any

### RequestBody - {
    "queries": {
        "id": "GUID",
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "login": "string",
    "restrictions": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-07-31T12:34:49.541Z",
        "expirationDate": "2023-07-31T12:34:49.541Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string"
        }
      }
    ],
    "ownerRestrictions": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-07-31T12:34:49.541Z",
        "expirationDate": "2023-07-31T12:34:49.541Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string"
        }
      }
    ],
    "additionalInfo": {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "imageUri": "string",
      "creationDate": "2023-07-31T12:34:49.541Z",
      "deletionDate": "2023-07-31T12:34:49.541Z",
      "role": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "string"
      },
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [M] Get -->

## GET - /api/user
Roles - User, Admin , Owner , Bot

### 200 ResponseBody - {
    "code": 0,
    "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "login": "string",
    "restrictions": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-07-31T12:34:49.541Z",
        "expirationDate": "2023-07-31T12:34:49.541Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string"
        }
      }
    ],
    "ownerRestrictions": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-07-31T12:34:49.541Z",
        "expirationDate": "2023-07-31T12:34:49.541Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string"
        }
      }
    ],
    "additionalInfo": {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "imageUri": "string",
      "creationDate": "2023-07-31T12:34:49.541Z",
      "deletionDate": "2023-07-31T12:34:49.541Z",
      "role": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "string"
      },
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [M] Get -->

## GET - /api/user/login/{login}
Roles - Any

### RequestBody - {
    "queries": {
        "login": "string",
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "login": "string",
    "restrictions": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-07-31T12:34:49.541Z",
        "expirationDate": "2023-07-31T12:34:49.541Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string"
        }
      }
    ],
    "ownerRestrictions": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-07-31T12:34:49.541Z",
        "expirationDate": "2023-07-31T12:34:49.541Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "name": "string"
        }
      }
    ],
    "additionalInfo": {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "imageUri": "string",
      "creationDate": "2023-07-31T12:34:49.541Z",
      "deletionDate": "2023-07-31T12:34:49.541Z",
      "role": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "string"
      },
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [C] UserAdditionalInfo -->
<!-- [M] Get -->

## GET - /api/user-additional-info/{id}
Roles - Any

### RequestBody - {
    "queries": {
        "id": "GUID",
    }
}

### 200 ResponseBody - {
    {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "imageUri": "string",
        "creationDate": "2023-07-31T12:39:51.607Z",
        "deletionDate": "2023-07-31T12:39:51.607Z",
        "role": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        },
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [M] GetByUserId -->

## GET - /api/user-additional-info/user/{id}
Roles - Any

### RequestBody - {
    "queries": {
        "id": "GUID",
    }
}

### 200 ResponseBody - {
    {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "imageUri": "string",
        "creationDate": "2023-07-31T12:39:51.607Z",
        "deletionDate": "2023-07-31T12:39:51.607Z",
        "role": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        },
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

<!-- [M] Get -->

## GET - /api/user-additional-info
Roles - User, Admin , Owner , Bot

### 200 ResponseBody - {
    {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "imageUri": "string",
        "creationDate": "2023-07-31T12:39:51.607Z",
        "deletionDate": "2023-07-31T12:39:51.607Z",
        "role": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        },
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [M] UpdateRole -->

## GET - /api/user-additional-info/role/{id}&{userId}
Roles - Owner

### RequestBody - {
    "queries": {
        "id": "GUID",
        "userId": "GUID"
    }
}

### 200 ResponseBody - {
    {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "imageUri": "string",
        "creationDate": "2023-07-31T12:39:51.607Z",
        "deletionDate": "2023-07-31T12:39:51.607Z",
        "role": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        },
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Роль не найдена"
    }
}

<!-- [M] UpdateDeletionDate -->

## GET - /api/user-additional-info/deletion/date/{userId}
Roles - Owner, Admin

### RequestBody - {
    "queries": {
        "userId": "GUID",
        "date": "null"
    }
}

### 200 ResponseBody - {
    {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "imageUri": "string",
        "creationDate": "2023-07-31T12:39:51.607Z",
        "deletionDate": "2023-07-31T12:39:51.607Z",
        "role": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        },
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 0,
        "message": "Дата не корректна"
    }
}

<!-- [M] UpdateImage -->

# GET - /api/user-additional-info/image/{uri}&{userId}
Roles - Owner, Admin

### RequestBody - {
    "queries": {
        "userId": "GUID",
        "uri": "string"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "imageUri": "string",
        "creationDate": "2023-07-31T12:39:51.607Z",
        "deletionDate": "2023-07-31T12:39:51.607Z",
        "role": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        },
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [M] UpdateImage -->

# GET - /api/user-additional-info/image/{uri}
Roles - User, Admin , Owner , Bot

### RequestBody - {
    "queries": {
        "uri": "string"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "imageUri": "string",
        "creationDate": "2023-07-31T12:39:51.607Z",
        "deletionDate": "2023-07-31T12:39:51.607Z",
        "role": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        },
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [C] UserRestriction -->
<!-- [M] Get -->

# GET - /api/user-restriction/{id}
Roles - Any

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Эффект не найден"
    }
}
<!-- [M] GetByUserId -->

# GET - /api/user-restriction/user/{id}
Roles - Any

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}
<!-- [M] GetByLogin -->

# GET - /api/user/login/{login}/restriction
Roles - Any

### RequestBody - {
    "queries": {
        "login": "GUID"
    }
}
### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}   
<!-- [M] Get -->

# GET - /api/user-restriction
Roles - User, Admin, Owner, Bot

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [M] GetByIds -->

# GET - /api/user-restriction/{userId}&{ownerId}
Roles - Any

### RequestBody - {
    "queries": {
        "userId": "GUID",
        "ownerId": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвиняемый не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвинитель не найден"
    }
}   
<!-- [M] GetByOwnerId -->

# GET - /api/user-restriction/owner/{id}
Roles - Any

### RequestBody - {
    "queries": {
        "id": "GUID",
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}   
<!-- [M] GetByAdmin -->

# GET - /api/user-restriction/owner
Roles - Admin, Owner, Bot

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [M] GetByAdminAndUserId -->

# GET - /api/user-restriction/{userId}/owner
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "userId": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвиняемый не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвинитель не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [M] GetRestrictionType -->

# GET - /api/user-restriction/types
Roles - Any

### 200 ResponseBody - {
    "code": 0,
    "data": [
        {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    ]
}
<!-- [M] Post -->

# POST - /api/user-restriction
Roles - Admin, Owner

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "creationDate": "2023-08-01T12:13:48.254Z",
    "expirationDate": "2023-08-01T12:13:48.254Z",
    "description": "string",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "typeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Тип эффекта не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвинитель не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвиняемый не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Эффект можно наложить только на пользователя"
    }
}
<!-- [M] Put -->

# PUT - /api/user-restriction
Roles - Admin, Owner

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "creationDate": "2023-08-01T12:25:01.110Z",
    "expirationDate": "2023-08-01T12:25:01.110Z",
    "description": "string",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "typeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Тип эффекта не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвинитель не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Обвиняемый не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Эффект не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Эффект можно наложить только на пользователя"
    }
}
<!-- Delete -->

# DELETE - /api/user-restriction/{id}
Roles - Admin, Owner 

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "creationDate": "2023-08-01T11:56:06.684Z",
        "expirationDate": "2023-08-01T11:56:06.684Z",
        "description": "string",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "type": {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Эффект не найден"
    }
}   

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}
<!-- [C] UserRole -->
<!-- [M] Get -->

# GET - /api/user-role/{id}
Roles - Any 

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "string"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Роль не найдена"
    }
}   
<!-- [M] Get -->

# GET - /api/user-role
Roles - Any 

### 200 ResponseBody - {
    "code": 0,
    "data": [
        {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "name": "string"
        }
    ]
}
# ===Authentication Service=== 
<!-- [C] AnswerImage -->
<!-- [M] Get -->

# GET - /api/answer-image/{id}
Roles - User, Admin, Owner, Bot 

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Картинка не найдена"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель сообщения"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
}  
<!-- [M] GetByAdmin -->

# GET - /api/answer-image/{id}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Картинка не найдена"
    }
}  
<!-- [M] GetByAnswerId -->

# GET - /api/answer-image/answer/{id}
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}


### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель сообщения"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByAdminAnswerId -->

# GET - /api/answer-image/answer/{id}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByTopicIdAsync -->

# GET - /api/answer-image/topic/{id}
Roles - User, Admin, Owner, Bot 

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель сообщения"
    }
}  
<!-- [M] GetByAdminTopicId -->

# GET - /api/answer-image/topic/{id}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Get -->

# GET - /api/answer-image
Roles - User, Admin, Owner, Bot

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByAdminUserId -->

# GET - /api/answer-image/user/{userId}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "userId": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Post -->

# POST - /api/answer-image
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель сообщения"
    }
} 
<!-- [M] Delete -->

# DELETE - /api/answer-image/{id}
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Картинка не найдена"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель сообщения"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] DeleteByAdmin -->

# DELETE - /api/user/topic/answer/image/{id}
Roles - Admin, Owner

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [С] SupportTopicAnswer -->
<!-- [M] Get -->

# GET - /api/support-topic-answer/{id}
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
}  

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель топика"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByAdmin -->

# GET - /api/support-topic-answer/{id}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Сообщение не найдено"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Get -->

# GET - /api/support-topic-answer
Roles - User, Admin, Owner, Bot

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByAdminUserId -->

# GET - /api/support-topic-answer/user/{userId}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "userId": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByTopicId -->

# GET - /api/support-topic-answer/topic/{id}
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByAdminTopicId -->

# GET - /api/support-topic-answer/topic/{id}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Post -->

# POST - /api/support-topic-answer
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "string",
    "content": "string",
    "date": "2023-08-01T13:30:23.470Z",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель топика"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] PostByAdmin -->

# POST - /api/support-topic-answer
Roles - Admin, Owner

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "string",
    "content": "string",
    "date": "2023-08-01T13:30:23.470Z",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Put -->

# PUT - /api/support-topic-answer
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "string",
    "content": "string",
    "date": "2023-08-01T13:30:23.470Z",
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Ответ не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель сообщения"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Delete -->

# DELETE - /api/support-topic-answer/{id}
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "date": "2023-08-01T13:32:39.829Z",
        "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ]  ,
        "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Ответ не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель сообщения"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [С] SupportTopic -->
<!-- [M] Get -->

# GET - /api/support-topic/{id}
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 4,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель топика"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByAdmin -->

# GET - /api/support-topic/{id}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByUserId -->

# GET - /api/support-topic
Roles - User, Admin, Owner, Bot

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetByAdminUserId -->

# GET - /api/support-topic/user/{userId}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "userId": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] GetOpenedTopics -->

# GET - /api/support-topic/opened
Roles - Admin, Owner, Bot

### 200 ResponseBody - {
    "code": 0,
    "data": [
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-02T13:55:36.734Z",
        "isClosed": true,
        "answers": [
        {
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "content": "string",
            "date": "2023-08-02T13:55:36.734Z",
            "images": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
            ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ]
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Close -->

# GET - /api/support-topic/{id}/close
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель топика"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] CloseByAdmin -->

# GET - /api/support-topic/{id}/close/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Post -->

# POST - /api/user/topic
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "string",
    "content": "string",
    "date": "2023-08-02T14:01:04.398Z",
    "isClosed": true,
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 5,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Put -->

# PUT - /api/user/topic
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "string",
    "content": "string",
    "date": "2023-08-02T14:01:04.398Z",
    "isClosed": true,
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 5,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель топика"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] Delete -->

# Delete - /api/user/topic/{id}
Roles - User, Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Пользователь не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 5,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Вы не создатель топика"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 
<!-- [M] DeleteByAdmin -->

# Delete - /api/user/topic/{id}/admin
Roles - Admin, Owner, Bot

### RequestBody - {
    "queries": {
        "id": "GUID"
    }
}

### 200 ResponseBody - {
    "code": 0,
    "data": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "title": "string",
        "content": "string",
        "date": "2023-08-01T13:45:09.320Z",
        "isClosed": true,
        "answers": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "content": "string",
                "date": "2023-08-01T13:45:09.320Z",
                "images": [
                    {
                    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "answerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                    }
                ],
            "plaintiffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            "topicId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
            }
        ],
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
}

### 400 ResponseBody - {
    "error": {
        "code": 5,
        "message": "Топик не найден"
    }
} 

### 400 ResponseBody - {
    "error": {
        "code": 1,
        "message": "Forbidden"
    }
} 