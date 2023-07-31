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
Roles - Owner

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