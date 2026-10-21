name: WebShop2 CI/CD

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    name: Build
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.0.x

      - name: Restore dependencies
        run: dotnet restore webShop2.csproj

      - name: Build application
        run: dotnet build webShop2.csproj -c Release --no-restore

  test:
    name: Testing
    runs-on: ubuntu-latest
    needs: build

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.0.x

      - name: Restore dependencies
        run: dotnet restore webShop2.csproj

      - name: Run tests
        run: dotnet test -c Release --no-restore

  docker:
    name: Docker Build
    runs-on: ubuntu-latest
    needs: test

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Build Docker image
        run: docker build -t webshop2:${{ github.sha }} .


