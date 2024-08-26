<h1 id="user-management">User Management v1</h1>
<h2 id="user-management-authentication">Authentication</h2>

Operations related to user authentication, including login, registration, token management, and password resets.

## Authenticates a user and provides a JWT token.

<a id="opIdPresentationWebApiEndpointsAuthenticationLogin"></a>

`POST /api/v1/auth/login`

> Body parameter

```json
{
  "username": "username",
  "password": "P@$$w0rd"
}
```

<h4 id="authenticates-a-user-and-provides-a-jwt-token.-parameters">Parameters</h4>

| Name     | In   | Type                                      | Required | Description                                    |
|----------|------|-------------------------------------------|----------|------------------------------------------------|
| body     | body | [LoginRequestDto](#schemaloginrequestdto) | true     | none                                           |
| username | body | string                                    | true     | The username of the user attempting to log in. |
| password | body | string                                    | true     | The password of the user attempting to log in. |

> Example responses

> 200 Response

```json
{
  "userId": "string",
  "tokenType": "string",
  "accessToken": "string",
  "accessTokenExpiresIn": 0,
  "refreshToken": "string",
  "refreshTokenExpiresIn": 0
}
```

<h4 id="authenticates-a-user-and-provides-a-jwt-token.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                        |
|--------|----------------------------------------------------------------------------|--------------|---------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [AuthenticationResponseDto](#schemaauthenticationresponsedto) |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails)   |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                          |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                       |

<aside class="success">
This operation does not require authentication
</aside>

## Refreshes the access token using a refresh token.

<a id="opIdPresentationWebApiEndpointsAuthenticationRefreshToken"></a>

`POST /api/v1/auth/refresh-token`

> Body parameter

```json
{
  "refreshToken": "token"
}
```

<h4 id="refreshes-the-access-token-using-a-refresh-token.-parameters">Parameters</h4>

| Name         | In   | Type                                                    | Required | Description                                          |
|--------------|------|---------------------------------------------------------|----------|------------------------------------------------------|
| body         | body | [RefreshTokenRequestDto](#schemarefreshtokenrequestdto) | true     | none                                                 |
| refreshToken | body | string                                                  | true     | The refresh token used to obtain a new access token. |

> Example responses

> 200 Response

```json
{
  "userId": "string",
  "tokenType": "string",
  "accessToken": "string",
  "accessTokenExpiresIn": 0,
  "refreshToken": "string",
  "refreshTokenExpiresIn": 0
}
```

<h4 id="refreshes-the-access-token-using-a-refresh-token.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                        |
|--------|----------------------------------------------------------------------------|--------------|---------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [AuthenticationResponseDto](#schemaauthenticationresponsedto) |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails)   |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                          |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                       |

<aside class="success">
This operation does not require authentication
</aside>

## Registers a new user.

<a id="opIdPresentationWebApiEndpointsAuthenticationRegister"></a>

`POST /api/v1/auth/register`

> Body parameter

```json
{
  "username": "username",
  "password": "P@$$w0rd",
  "confirmPassword": "P@$$w0rd",
  "emailAddress": "username@mail.com"
}
```

<h4 id="registers-a-new-user.-parameters">Parameters</h4>

| Name            | In   | Type                                            | Required | Description                                                      |
|-----------------|------|-------------------------------------------------|----------|------------------------------------------------------------------|
| body            | body | [RegisterRequestDto](#schemaregisterrequestdto) | true     | none                                                             |
| username        | body | string                                          | true     | The desired username for the new user account.                   |
| password        | body | string                                          | true     | The password for the new user account.                           |
| confirmPassword | body | string                                          | true     | Confirmation of the password to ensure it was entered correctly. |
| emailAddress    | body | string                                          | true     | The email address for the new user account.                      |

> Example responses

> 200 Response

```json
{
  "userId": "string",
  "tokenType": "string",
  "accessToken": "string",
  "accessTokenExpiresIn": 0,
  "refreshToken": "string",
  "refreshTokenExpiresIn": 0
}
```

<h4 id="registers-a-new-user.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                        |
|--------|----------------------------------------------------------------------------|--------------|---------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [AuthenticationResponseDto](#schemaauthenticationresponsedto) |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails)   |
| 409    | [Conflict](https://tools.ietf.org/html/rfc7231#section-6.5.8)              | none         | None                                                          |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                       |

<aside class="success">
This operation does not require authentication
</aside>

## Requests a password reset for the specified email address.

<a id="opIdPresentationWebApiEndpointsAuthenticationRequestResetPassword"></a>

`POST /api/v1/auth/request-reset-password`

> Body parameter

```json
{
  "emailAddress": "username@mail.com"
}
```

<h4 id="requests-a-password-reset-for-the-specified-email-address.-parameters">Parameters</h4>

| Name         | In   | Type                                                                    | Required | Description                                                                       |
|--------------|------|-------------------------------------------------------------------------|----------|-----------------------------------------------------------------------------------|
| body         | body | [RequestResetPasswordRequestDto](#schemarequestresetpasswordrequestdto) | true     | none                                                                              |
| emailAddress | body | string                                                                  | true     | The email address associated with the user account requesting the password reset. |

> Example responses

> 200 Response

```
null
```

```json
null
```

<h4 id="requests-a-password-reset-for-the-specified-email-address.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                      |
|--------|----------------------------------------------------------------------------|--------------|-------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | Inline                                                      |
| 202    | [Accepted](https://tools.ietf.org/html/rfc7231#section-6.3.3)              | Accepted     | None                                                        |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails) |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                     |

<h4 id="requests-a-password-reset-for-the-specified-email-address.-responseschema">Response Schema</h4>

<aside class="success">
This operation does not require authentication
</aside>

## Resets the password using a password reset token.

<a id="opIdPresentationWebApiEndpointsAuthenticationResetPassword"></a>

`POST /api/v1/auth/reset-password`

> Body parameter

```json
{
  "passwordResetToken": "token",
  "password": "P@$$w0rd",
  "confirmPassword": "P@$$w0rd"
}
```

<h4 id="resets-the-password-using-a-password-reset-token.-parameters">Parameters</h4>

| Name               | In   | Type                                                      | Required | Description                                                          |
|--------------------|------|-----------------------------------------------------------|----------|----------------------------------------------------------------------|
| body               | body | [ResetPasswordRequestDto](#schemaresetpasswordrequestdto) | true     | none                                                                 |
| passwordResetToken | body | string                                                    | true     | The token provided to the user for password reset.                   |
| password           | body | string                                                    | true     | The new password the user wants to set.                              |
| confirmPassword    | body | string                                                    | true     | Confirmation of the new password to ensure it was entered correctly. |

> Example responses

> 200 Response

```
null
```

```json
null
```

<h4 id="resets-the-password-using-a-password-reset-token.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                      |
|--------|----------------------------------------------------------------------------|--------------|-------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | Inline                                                      |
| 204    | [No Content](https://tools.ietf.org/html/rfc7231#section-6.3.5)            | No Content   | None                                                        |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails) |
| 404    | [Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)             | Not Found    | None                                                        |
| 409    | [Conflict](https://tools.ietf.org/html/rfc7231#section-6.5.8)              | none         | None                                                        |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                     |

<h4 id="resets-the-password-using-a-password-reset-token.-responseschema">Response Schema</h4>

<aside class="success">
This operation does not require authentication
</aside>

## Revokes a refresh token to prevent further use.

<a id="opIdPresentationWebApiEndpointsAuthenticationRevokeRefreshToken"></a>

`POST /api/v1/auth/revoke-refresh-token`

> Body parameter

```json
{
  "refreshToken": "token"
}
```

<h4 id="revokes-a-refresh-token-to-prevent-further-use.-parameters">Parameters</h4>

| Name         | In   | Type                                                    | Required | Description                                          |
|--------------|------|---------------------------------------------------------|----------|------------------------------------------------------|
| body         | body | [RefreshTokenRequestDto](#schemarefreshtokenrequestdto) | true     | none                                                 |
| refreshToken | body | string                                                  | true     | The refresh token used to obtain a new access token. |

> Example responses

> 200 Response

```
null
```

```json
null
```

<h4 id="revokes-a-refresh-token-to-prevent-further-use.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                      |
|--------|----------------------------------------------------------------------------|--------------|-------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | Inline                                                      |
| 204    | [No Content](https://tools.ietf.org/html/rfc7231#section-6.3.5)            | No Content   | None                                                        |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                        |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                     |

<h4 id="revokes-a-refresh-token-to-prevent-further-use.-responseschema">Response Schema</h4>

<aside class="success">
This operation does not require authentication
</aside>

<h2 id="user-management-user">User</h2>

Operations related to the current user's profile, including retrieving and updating personal information.

## Updates the current user's password.

<a id="opIdPresentationWebApiEndpointsAuthenticatedUserChangePassword"></a>

`PATCH /api/v1/user/password`

> Body parameter

```json
{
  "password": "P@$$w0rd",
  "confirmPassword": "P@$$w0rd"
}
```

<h4 id="updates-the-current-user's-password.-parameters">Parameters</h4>

| Name            | In   | Type                                                        | Required | Description                                                          |
|-----------------|------|-------------------------------------------------------------|----------|----------------------------------------------------------------------|
| body            | body | [ChangePasswordRequestDto](#schemachangepasswordrequestdto) | true     | none                                                                 |
| password        | body | string                                                      | true     | The new password the user wants to set.                              |
| confirmPassword | body | string                                                      | true     | Confirmation of the new password to ensure it was entered correctly. |

> Example responses

> 200 Response

```json
{
  "userId": "string",
  "username": "string",
  "emailAddress": "string",
  "userRole": "string",
  "createdBy": "string",
  "createdAt": "string",
  "updatedBy": "string",
  "updatedAt": "string"
}
```

<h4 id="updates-the-current-user's-password.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                      |
|--------|----------------------------------------------------------------------------|--------------|-------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [UserProfileResponseDto](#schemauserprofileresponsedto)     |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                        |
| 409    | [Conflict](https://tools.ietf.org/html/rfc7231#section-6.5.8)              | none         | None                                                        |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                     |

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth
</aside>

## Retrieves the profile information of the currently authenticated user.

<a id="opIdPresentationWebApiEndpointsAuthenticatedUserGet"></a>

`GET /api/v1/user`

> Example responses

> 200 Response

```json
{
  "userId": "string",
  "username": "string",
  "emailAddress": "string",
  "userRole": "string",
  "createdBy": "string",
  "createdAt": "string",
  "updatedBy": "string",
  "updatedAt": "string"
}
```

<h4 id="retrieves-the-profile-information-of-the-currently-authenticated-user.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                  |
|--------|----------------------------------------------------------------------------|--------------|---------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [UserProfileResponseDto](#schemauserprofileresponsedto) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                    |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                 |

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth
</aside>

## Updates the current user's profile information.

<a id="opIdPresentationWebApiEndpointsAuthenticatedUserUpdate"></a>

`PUT /api/v1/user`

> Body parameter

```json
{
  "username": "username",
  "emailAddress": "username@mail.com"
}
```

<h4 id="updates-the-current-user's-profile-information.-parameters">Parameters</h4>

| Name         | In   | Type                                                | Required | Description                         |
|--------------|------|-----------------------------------------------------|----------|-------------------------------------|
| body         | body | [UpdateUserRequestDto](#schemaupdateuserrequestdto) | true     | none                                |
| username     | body | string¦null                                         | false    | The new username for the user.      |
| emailAddress | body | string¦null                                         | false    | The new email address for the user. |

> Example responses

> 200 Response

```json
{
  "userId": "string",
  "username": "string",
  "emailAddress": "string",
  "userRole": "string",
  "createdBy": "string",
  "createdAt": "string",
  "updatedBy": "string",
  "updatedAt": "string"
}
```

<h4 id="updates-the-current-user's-profile-information.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                      |
|--------|----------------------------------------------------------------------------|--------------|-------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [UserProfileResponseDto](#schemauserprofileresponsedto)     |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                        |
| 409    | [Conflict](https://tools.ietf.org/html/rfc7231#section-6.5.8)              | none         | None                                                        |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                     |

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth
</aside>

<h2 id="user-management-user-management">User Management</h2>

Operations for managing users in the system, listing, updating, and deleting user accounts.

## Deletes a user with the specified userId.

<a id="opIdPresentationWebApiEndpointsUserManagementDeleteUser"></a>

`DELETE /api/v1/user-management/users/{userId}`

<h4 id="deletes-a-user-with-the-specified-userid.-parameters">Parameters</h4>

| Name   | In   | Type         | Required | Description                         |
|--------|------|--------------|----------|-------------------------------------|
| userId | path | string(guid) | true     | The unique identifier for the user. |

> Example responses

> 200 Response

```
null
```

```json
null
```

<h4 id="deletes-a-user-with-the-specified-userid.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                      |
|--------|----------------------------------------------------------------------------|--------------|-------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | Inline                                                      |
| 204    | [No Content](https://tools.ietf.org/html/rfc7231#section-6.3.5)            | No Content   | None                                                        |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                        |
| 403    | [Forbidden](https://tools.ietf.org/html/rfc7231#section-6.5.3)             | Forbidden    | None                                                        |
| 404    | [Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)             | Not Found    | None                                                        |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                     |

<h4 id="deletes-a-user-with-the-specified-userid.-responseschema">Response Schema</h4>

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth ( Scopes: Admin )
</aside>

## Updates the profile information of a user with the specified userId.

<a id="opIdPresentationWebApiEndpointsUserManagementUpdateUser"></a>

`PUT /api/v1/user-management/users/{userId}`

> Body parameter

```json
{
  "username": "username",
  "emailAddress": "username@mail.com"
}
```

<h4 id="updates-the-profile-information-of-a-user-with-the-specified-userid.-parameters">Parameters</h4>

| Name         | In   | Type                                                | Required | Description                         |
|--------------|------|-----------------------------------------------------|----------|-------------------------------------|
| userId       | path | string(guid)                                        | true     | none                                |
| body         | body | [UpdateUserRequestDto](#schemaupdateuserrequestdto) | true     | none                                |
| username     | body | string¦null                                         | false    | The new username for the user.      |
| emailAddress | body | string¦null                                         | false    | The new email address for the user. |

> Example responses

> 200 Response

```json
{
  "userId": "string",
  "username": "string",
  "emailAddress": "string",
  "userRole": "string",
  "createdBy": "string",
  "createdAt": "string",
  "updatedBy": "string",
  "updatedAt": "string"
}
```

<h4 id="updates-the-profile-information-of-a-user-with-the-specified-userid.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                      |
|--------|----------------------------------------------------------------------------|--------------|-------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [UserProfileResponseDto](#schemauserprofileresponsedto)     |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                        |
| 403    | [Forbidden](https://tools.ietf.org/html/rfc7231#section-6.5.3)             | Forbidden    | None                                                        |
| 404    | [Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)             | Not Found    | None                                                        |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                     |

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth ( Scopes: Admin )
</aside>

## Retrieves a paginated list of users.

<a id="opIdPresentationWebApiEndpointsUserManagementGetUsers"></a>

`GET /api/v1/user-management/users`

<h4 id="retrieves-a-paginated-list-of-users.-parameters">Parameters</h4>

| Name        | In    | Type           | Required | Description                                   |
|-------------|-------|----------------|----------|-----------------------------------------------|
| pageNumber  | query | integer(int32) | false    | The page number to retrieve, starting from 1. |
| pageSize    | query | integer(int32) | false    | The number of items to include per page.      |
| sortBy      | query | string         | false    | The field by which to sort the results.       |
| sortOrder   | query | string         | false    | The order in which to sort the results.       |
| searchField | query | string         | false    | The specific field to search within.          |
| searchTerm  | query | string         | false    | The term to search for in the results.        |

> Example responses

> 200 Response

```json
{
  "metadata": {
    "pageNumber": 0,
    "pageSize": 0,
    "pageCount": 0,
    "totalCount": 0,
    "hasMore": true
  },
  "data": [
    {
      "userId": "string",
      "username": "string",
      "emailAddress": "string",
      "userRole": "string",
      "createdBy": "string",
      "createdAt": "string",
      "updatedBy": "string",
      "updatedAt": "string"
    }
  ]
}
```

<h4 id="retrieves-a-paginated-list-of-users.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                                                                      |
|--------|----------------------------------------------------------------------------|--------------|---------------------------------------------------------------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [PagedResponseDtoOfUserProfileResponseDto](#schemapagedresponsedtoofuserprofileresponsedto) |
| 400    | [Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)           | Bad Request  | [ValidationProblemDetails](#schemavalidationproblemdetails)                                 |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                                                                        |
| 403    | [Forbidden](https://tools.ietf.org/html/rfc7231#section-6.5.3)             | Forbidden    | None                                                                                        |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails)                                                     |

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth ( Scopes: Admin )
</aside>

## Retrieves fields which can be used to search users.

<a id="opIdPresentationWebApiEndpointsUserManagementGetUsersSearchableFields"></a>

`GET /api/v1/user-management/users/searchable-fields`

> Example responses

> 200 Response

```json
{
  "fields": [
    "string"
  ]
}
```

<h4 id="retrieves-fields-which-can-be-used-to-search-users.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                  |
|--------|----------------------------------------------------------------------------|--------------|-----------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [QueryFieldsDto](#schemaqueryfieldsdto) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                    |
| 403    | [Forbidden](https://tools.ietf.org/html/rfc7231#section-6.5.3)             | Forbidden    | None                                    |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails) |

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth ( Scopes: Admin )
</aside>

## Retrieves fields which can be used to sort users.

<a id="opIdPresentationWebApiEndpointsUserManagementGetUsersSortableFields"></a>

`GET /api/v1/user-management/users/sortable-fields`

> Example responses

> 200 Response

```json
{
  "fields": [
    "string"
  ]
}
```

<h4 id="retrieves-fields-which-can-be-used-to-sort-users.-responses">Responses</h4>

| Status | Meaning                                                                    | Description  | Schema                                  |
|--------|----------------------------------------------------------------------------|--------------|-----------------------------------------|
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)                    | Success      | [QueryFieldsDto](#schemaqueryfieldsdto) |
| 401    | [Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)            | Unauthorized | None                                    |
| 403    | [Forbidden](https://tools.ietf.org/html/rfc7231#section-6.5.3)             | Forbidden    | None                                    |
| 500    | [Internal Server Error](https://tools.ietf.org/html/rfc7231#section-6.6.1) | Server Error | [ProblemDetails](#schemaproblemdetails) |

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
JWTBearerAuth ( Scopes: Admin )
</aside>

# Schemas

<h3 id="tocS_ValidationProblemDetails">ValidationProblemDetails</h3>
<!-- backwards compatibility -->
<a id="schemavalidationproblemdetails"></a>
<a id="schema_ValidationProblemDetails"></a>
<a id="tocSvalidationproblemdetails"></a>
<a id="tocsvalidationproblemdetails"></a>

```json
{
  "errors": {
    "property1": [
      "string"
    ],
    "property2": [
      "string"
    ]
  },
  "type": "string",
  "title": "string",
  "status": 0,
  "detail": "string",
  "instance": "string",
  "property1": null,
  "property2": null
}

```

### Properties

| Name                       | Type                | Required | Restrictions | Description |
|----------------------------|---------------------|----------|--------------|-------------|
| **additionalProperties**   | any                 | false    | none         | none        |
| errors                     | object              | false    | none         | none        |
| » **additionalProperties** | [string]            | false    | none         | none        |
| type                       | string¦null         | false    | none         | none        |
| title                      | string¦null         | false    | none         | none        |
| status                     | integer(int32)¦null | false    | none         | none        |
| detail                     | string¦null         | false    | none         | none        |
| instance                   | string¦null         | false    | none         | none        |

<h3 id="tocS_ProblemDetails">ProblemDetails</h3>
<!-- backwards compatibility -->
<a id="schemaproblemdetails"></a>
<a id="schema_ProblemDetails"></a>
<a id="tocSproblemdetails"></a>
<a id="tocsproblemdetails"></a>

```json
{
  "type": "string",
  "title": "string",
  "status": 0,
  "detail": "string",
  "instance": "string",
  "property1": null,
  "property2": null
}

```

### Properties

| Name                     | Type                | Required | Restrictions | Description |
|--------------------------|---------------------|----------|--------------|-------------|
| **additionalProperties** | any                 | false    | none         | none        |
| type                     | string¦null         | false    | none         | none        |
| title                    | string¦null         | false    | none         | none        |
| status                   | integer(int32)¦null | false    | none         | none        |
| detail                   | string¦null         | false    | none         | none        |
| instance                 | string¦null         | false    | none         | none        |

<h3 id="tocS_PagedResponseDtoOfUserProfileResponseDto">PagedResponseDtoOfUserProfileResponseDto</h3>
<!-- backwards compatibility -->
<a id="schemapagedresponsedtoofuserprofileresponsedto"></a>
<a id="schema_PagedResponseDtoOfUserProfileResponseDto"></a>
<a id="tocSpagedresponsedtoofuserprofileresponsedto"></a>
<a id="tocspagedresponsedtoofuserprofileresponsedto"></a>

```json
{
  "metadata": {
    "pageNumber": 0,
    "pageSize": 0,
    "pageCount": 0,
    "totalCount": 0,
    "hasMore": true
  },
  "data": [
    {
      "userId": "string",
      "username": "string",
      "emailAddress": "string",
      "userRole": "string",
      "createdBy": "string",
      "createdAt": "string",
      "updatedBy": "string",
      "updatedAt": "string"
    }
  ]
}

```

DTO for paginated responses, including metadata and data for a specific type.

### Properties

| Name     | Type                                                        | Required | Restrictions | Description                                                                  |
|----------|-------------------------------------------------------------|----------|--------------|------------------------------------------------------------------------------|
| metadata | [PagedResponseMetadataDto](#schemapagedresponsemetadatadto) | true     | none         | Metadata about the pagination, such as total count and current page details. |
| data     | [[UserProfileResponseDto](#schemauserprofileresponsedto)]   | true     | none         | The data items for the current page.                                         |

<h3 id="tocS_PagedResponseMetadataDto">PagedResponseMetadataDto</h3>
<!-- backwards compatibility -->
<a id="schemapagedresponsemetadatadto"></a>
<a id="schema_PagedResponseMetadataDto"></a>
<a id="tocSpagedresponsemetadatadto"></a>
<a id="tocspagedresponsemetadatadto"></a>

```json
{
  "pageNumber": 0,
  "pageSize": 0,
  "pageCount": 0,
  "totalCount": 0,
  "hasMore": true
}

```

Metadata for paginated responses, providing details about pagination and result set.

### Properties

| Name       | Type           | Required | Restrictions | Description                                                                         |
|------------|----------------|----------|--------------|-------------------------------------------------------------------------------------|
| pageNumber | integer(int32) | true     | none         | The current page number in the paginated result.                                    |
| pageSize   | integer(int32) | true     | none         | The number of items per page.                                                       |
| pageCount  | integer(int32) | true     | none         | The total number of pages available based on the current page size and total count. |
| totalCount | integer(int32) | true     | none         | The total number of items available across all pages.                               |
| hasMore    | boolean        | true     | none         | Indicates whether there are more pages of data available beyond the current page.   |

<h3 id="tocS_UserProfileResponseDto">UserProfileResponseDto</h3>
<!-- backwards compatibility -->
<a id="schemauserprofileresponsedto"></a>
<a id="schema_UserProfileResponseDto"></a>
<a id="tocSuserprofileresponsedto"></a>
<a id="tocsuserprofileresponsedto"></a>

```json
{
  "userId": "string",
  "username": "string",
  "emailAddress": "string",
  "userRole": "string",
  "createdBy": "string",
  "createdAt": "string",
  "updatedBy": "string",
  "updatedAt": "string"
}

```

DTO for user profile information response.

### Properties

| Name         | Type         | Required | Restrictions | Description                                                   |
|--------------|--------------|----------|--------------|---------------------------------------------------------------|
| userId       | string(guid) | true     | none         | The unique identifier for the user.                           |
| username     | string       | true     | none         | The username of the user.                                     |
| emailAddress | string       | true     | none         | The email address of the user.                                |
| userRole     | string       | true     | none         | The role assigned to the user.                                |
| createdBy    | string       | true     | none         | The username of the person who created the user profile.      |
| createdAt    | string       | true     | none         | The date and time when the user profile was created.          |
| updatedBy    | string       | true     | none         | The username of the person who last updated the user profile. |
| updatedAt    | string       | true     | none         | The date and time when the user profile was last updated.     |

<h3 id="tocS_QueryFieldsDto">QueryFieldsDto</h3>
<!-- backwards compatibility -->
<a id="schemaqueryfieldsdto"></a>
<a id="schema_QueryFieldsDto"></a>
<a id="tocSqueryfieldsdto"></a>
<a id="tocsqueryfieldsdto"></a>

```json
{
  "fields": [
    "string"
  ]
}

```

DTO for specifying fields that can be used for searching or sorting.

### Properties

| Name   | Type     | Required | Restrictions | Description                                                                |
|--------|----------|----------|--------------|----------------------------------------------------------------------------|
| fields | [string] | true     | none         | An array of field names that can be used for search or sorting operations. |

<h3 id="tocS_UpdateUserRequestDto">UpdateUserRequestDto</h3>
<!-- backwards compatibility -->
<a id="schemaupdateuserrequestdto"></a>
<a id="schema_UpdateUserRequestDto"></a>
<a id="tocSupdateuserrequestdto"></a>
<a id="tocsupdateuserrequestdto"></a>

```json
{
  "username": "username",
  "emailAddress": "username@mail.com"
}

```

DTO for updating user information.

### Properties

| Name         | Type        | Required | Restrictions | Description                         |
|--------------|-------------|----------|--------------|-------------------------------------|
| username     | string¦null | false    | none         | The new username for the user.      |
| emailAddress | string¦null | false    | none         | The new email address for the user. |

<h3 id="tocS_AuthenticationResponseDto">AuthenticationResponseDto</h3>
<!-- backwards compatibility -->
<a id="schemaauthenticationresponsedto"></a>
<a id="schema_AuthenticationResponseDto"></a>
<a id="tocSauthenticationresponsedto"></a>
<a id="tocsauthenticationresponsedto"></a>

```json
{
  "userId": "string",
  "tokenType": "string",
  "accessToken": "string",
  "accessTokenExpiresIn": 0,
  "refreshToken": "string",
  "refreshTokenExpiresIn": 0
}

```

DTO for the authentication response, containing tokens and expiration details.

### Properties

| Name                  | Type           | Required | Restrictions | Description                                                      |
|-----------------------|----------------|----------|--------------|------------------------------------------------------------------|
| userId                | string(guid)   | true     | none         | The unique identifier for the authenticated user.                |
| tokenType             | string         | true     | none         | The type of token issued (e.g., Bearer).                         |
| accessToken           | string         | true     | none         | The access token issued for the authenticated user.              |
| accessTokenExpiresIn  | integer(int32) | true     | none         | The duration in seconds until the access token expires.          |
| refreshToken          | string         | true     | none         | The refresh token that can be used to obtain a new access token. |
| refreshTokenExpiresIn | integer(int32) | true     | none         | The duration in seconds until the refresh token expires.         |

<h3 id="tocS_LoginRequestDto">LoginRequestDto</h3>
<!-- backwards compatibility -->
<a id="schemaloginrequestdto"></a>
<a id="schema_LoginRequestDto"></a>
<a id="tocSloginrequestdto"></a>
<a id="tocsloginrequestdto"></a>

```json
{
  "username": "username",
  "password": "P@$$w0rd"
}

```

DTO for user login request containing username and password.

### Properties

| Name     | Type   | Required | Restrictions | Description                                    |
|----------|--------|----------|--------------|------------------------------------------------|
| username | string | true     | none         | The username of the user attempting to log in. |
| password | string | true     | none         | The password of the user attempting to log in. |

<h3 id="tocS_RefreshTokenRequestDto">RefreshTokenRequestDto</h3>
<!-- backwards compatibility -->
<a id="schemarefreshtokenrequestdto"></a>
<a id="schema_RefreshTokenRequestDto"></a>
<a id="tocSrefreshtokenrequestdto"></a>
<a id="tocsrefreshtokenrequestdto"></a>

```json
{
  "refreshToken": "token"
}

```

DTO for requesting a new access token using a refresh token.

### Properties

| Name         | Type   | Required | Restrictions | Description                                          |
|--------------|--------|----------|--------------|------------------------------------------------------|
| refreshToken | string | true     | none         | The refresh token used to obtain a new access token. |

<h3 id="tocS_RegisterRequestDto">RegisterRequestDto</h3>
<!-- backwards compatibility -->
<a id="schemaregisterrequestdto"></a>
<a id="schema_RegisterRequestDto"></a>
<a id="tocSregisterrequestdto"></a>
<a id="tocsregisterrequestdto"></a>

```json
{
  "username": "username",
  "password": "P@$$w0rd",
  "confirmPassword": "P@$$w0rd",
  "emailAddress": "username@mail.com"
}

```

DTO for user registration request containing username, password, and email address.

### Properties

| Name            | Type   | Required | Restrictions | Description                                                      |
|-----------------|--------|----------|--------------|------------------------------------------------------------------|
| username        | string | true     | none         | The desired username for the new user account.                   |
| password        | string | true     | none         | The password for the new user account.                           |
| confirmPassword | string | true     | none         | Confirmation of the password to ensure it was entered correctly. |
| emailAddress    | string | true     | none         | The email address for the new user account.                      |

<h3 id="tocS_RequestResetPasswordRequestDto">RequestResetPasswordRequestDto</h3>
<!-- backwards compatibility -->
<a id="schemarequestresetpasswordrequestdto"></a>
<a id="schema_RequestResetPasswordRequestDto"></a>
<a id="tocSrequestresetpasswordrequestdto"></a>
<a id="tocsrequestresetpasswordrequestdto"></a>

```json
{
  "emailAddress": "username@mail.com"
}

```

DTO for requesting a password reset, containing the user's email address.

### Properties

| Name         | Type   | Required | Restrictions | Description                                                                       |
|--------------|--------|----------|--------------|-----------------------------------------------------------------------------------|
| emailAddress | string | true     | none         | The email address associated with the user account requesting the password reset. |

<h3 id="tocS_ResetPasswordRequestDto">ResetPasswordRequestDto</h3>
<!-- backwards compatibility -->
<a id="schemaresetpasswordrequestdto"></a>
<a id="schema_ResetPasswordRequestDto"></a>
<a id="tocSresetpasswordrequestdto"></a>
<a id="tocsresetpasswordrequestdto"></a>

```json
{
  "passwordResetToken": "token",
  "password": "P@$$w0rd",
  "confirmPassword": "P@$$w0rd"
}

```

DTO for resetting a user's password, including the reset token and new password details.

### Properties

| Name               | Type   | Required | Restrictions | Description                                                          |
|--------------------|--------|----------|--------------|----------------------------------------------------------------------|
| passwordResetToken | string | true     | none         | The token provided to the user for password reset.                   |
| password           | string | true     | none         | The new password the user wants to set.                              |
| confirmPassword    | string | true     | none         | Confirmation of the new password to ensure it was entered correctly. |

<h3 id="tocS_ChangePasswordRequestDto">ChangePasswordRequestDto</h3>
<!-- backwards compatibility -->
<a id="schemachangepasswordrequestdto"></a>
<a id="schema_ChangePasswordRequestDto"></a>
<a id="tocSchangepasswordrequestdto"></a>
<a id="tocschangepasswordrequestdto"></a>

```json
{
  "password": "P@$$w0rd",
  "confirmPassword": "P@$$w0rd"
}

```

DTO for changing the user's password.

### Properties

| Name            | Type   | Required | Restrictions | Description                                                          |
|-----------------|--------|----------|--------------|----------------------------------------------------------------------|
| password        | string | true     | none         | The new password the user wants to set.                              |
| confirmPassword | string | true     | none         | Confirmation of the new password to ensure it was entered correctly. |

