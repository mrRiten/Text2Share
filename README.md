# Text2Share

Text2Share is a platform for sharing texts with a microservice architecture, including user registration with email confirmation.

## Contents

1. [Description](#description)
2. [Architecture](#architecture)
3. [Installation](#installation)
4. [Microservices](#microservices)
    - [AuthorizeMicroService](#authormicroservice)
    - [EmailMicroService](#emailmicroservice)
    - [TextMicroService](#textmicroservice)
    - [UserMicroService](#usermicroservice)
5. [Usage](#usage)

## Description

Text2Share allows users to share various texts. The platform consists of several microservices, each responsible for a specific part of the functionality.

## Architecture

The project is built on a microservice architecture where each service performs specific tasks:
- **AuthorizeMicroService**: Manages user authorization and authentication.
- **EmailMicroService**: Sends emails for registration confirmation and other notifications.
- **TextMicroService**: Manages texts that users can publish and share.
- **UserMicroService**: Manages user data.

All MicroServices has Onion Architecture:
- **\<MicroServiceName>.Core** (Domain) contains: models, dbContext, attributes.
- **\<MicroServiceName>.Application** contains: all services/repositories contracts (interfaces).
- **\<MicroServiceName>.Infrastructure** contains: realization of contracts and HostHelpers.
- **\<MicroServiceName>.API** contains: RESTfull API Controllers with methods, configure of MicroService. 

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/mrRiten/Text2Share.git
    cd text2share
    ```

2. Set up each microservice in its own directory:

### AuthorizeMicroService

3. Navigate to the `AuthorizeMicroService` directory and configure the `appsettings.json` file with your values.

4. Run database migrations:
    ```sh
    dotnet ef database update
    ```

5. Run the service:
    ```sh
    dotnet run
    ```

### EmailMicroService

3. Navigate to the `EmailMicroService` directory and configure the `appsettings.json` file with your values.

4. Run database migrations:
    ```sh
    dotnet ef database update
    ```

5. Run the service:
    ```sh
    dotnet run
    ```

### TextMicroService

3. Navigate to the `TextMicroService` directory and configure the `appsettings.json` file with your values.

4. Run database migrations:
    ```sh
    dotnet ef database update
    ```

5. Run the service:
    ```sh
    dotnet run
    ```

### UserMicroService

3. Navigate to the `UserMicroService` directory and configure the `appsettings.json` file with your values.

4. Run database migrations:
    ```sh
    dotnet ef database update
    ```

5. Run the service:
    ```sh
    dotnet run
    ```

## Microservices

### AuthorizeMicroService

Manages authentication, authorization, and user confirmation requests.

#### Configuration

- JWT settings in `appsettings.json`:
    ```json
    {
      "JwtSettings": {
        "Issuer": "your_issuer",
        "Audience": "your_audience",
        "SecretKey": "your_secret_key"
      }
    }
    ```

### EmailMicroService

Sends and creates confirmation emails.
(Sending messages is handled by a separate service)

#### Configuration

- SMTP settings in `appsettings.json`:
    ```json
    {
      "SMTP": {
        "Server": "your_smtp_server",
        "Port": 465,
        "Address": "your_smtp_username",
        "Password": "your_smtp_password",
        "Name": "your_product_name"
      }
    }
    ```

### TextMicroService

Manages texts that users can share.

#### Configuration

- JWT settings in `appsettings.json`:
    ```json
    {
      "JwtSettings": {
        "Issuer": "your_issuer",
        "Audience": "your_audience",
        "SecretKey": "your_secret_key"
      }
    }
    ```

### UserMicroService

Manages user data.

#### Configuration

- Database connection settings in `appsettings.json`:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "your_connection_string"
      }
    }
    ```

## Usage

After starting all microservices, you can use the platform through the provided APIs.
All MicroServices has Swagger for demonstration API methods.
