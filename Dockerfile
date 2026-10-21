name: WebShop2 CI/CD

on:
  push:
    branches:
      - main
  pull_request:
    branches:
      - main
  workflow_dispatch:
    inputs:
      deploy_production:
        description: "Deploy to production"
        required: true
        type: boolean
        default: false

permissions:
  contents: read

env:
  IMAGE_NAME: ghcr.io/vukasinstupar/webshop2

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

      - name: Restore application
        run: dotnet restore webShop2.csproj

      - name: Run tests
        shell: bash
        run: |
          mapfile -t test_projects < <(
            find . -type f -name '*.csproj' \
              ! -path './bin/*' \
              ! -path './obj/*' \
              ! -path '*/bin/*' \
              ! -path '*/obj/*' \
              | grep -Ei '(^|/)[^/]*(test|tests)[^/]*\.csproj$' \
              || true
          )

          if [ ${#test_projects[@]} -eq 0 ]; then
            echo "::warning::No test project found. Add a dedicated test project to enable automated tests."
            exit 0
          fi

          for project in "${test_projects[@]}"; do
            dotnet test "$project" \
              --configuration Release \
              --logger "console;verbosity=normal"
          done

  docker:
    name: Docker Build and Publish
    runs-on: ubuntu-latest
    needs: test
    if: github.event_name != 'pull_request'

    permissions:
      contents: read
      packages: write

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup Docker Buildx
        uses: docker/setup-buildx-action@v3

      - name: Login to GitHub Container Registry
        uses: docker/login-action@v3
        with:
          registry: ghcr.io
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Extract Docker metadata
        id: meta
        uses: docker/metadata-action@v5
        with:
          images: ${{ env.IMAGE_NAME }}
          tags: |
            type=raw,value=latest,enable={{is_default_branch}}
            type=sha,prefix=sha-

      - name: Build and push Docker image
        uses: docker/build-push-action@v6
        with:
          context: .
          file: ./Dockerfile
          push: true
          tags: ${{ steps.meta.outputs.tags }}
          labels: ${{ steps.meta.outputs.labels }}

  deploy_staging:
    name: Deploy to Staging
    runs-on: ubuntu-latest
    needs: docker
    if: github.event_name == 'push' && github.ref == 'refs/heads/main'
    environment: staging

    steps:
      - name: Deploy staging
        uses: appleboy/ssh-action@v1.2.2
        env:
          IMAGE: ${{ env.IMAGE_NAME }}:sha-${{ github.sha }}
          REGISTRY_USER: ${{ secrets.GHCR_USERNAME }}
          REGISTRY_TOKEN: ${{ secrets.GHCR_TOKEN }}
          DB_CONNECTION: ${{ secrets.STAGING_DB_CONNECTION }}
        with:
          host: ${{ secrets.STAGING_HOST }}
          username: ${{ secrets.STAGING_USER }}
          key: ${{ secrets.STAGING_SSH_KEY }}
          envs: IMAGE,REGISTRY_USER,REGISTRY_TOKEN,DB_CONNECTION
          script: |
            set -e
            echo "$REGISTRY_TOKEN" | docker login ghcr.io \
              -u "$REGISTRY_USER" --password-stdin
            docker pull "$IMAGE"
            docker stop webshop2-staging || true
            docker rm webshop2-staging || true
            docker run -d \
              --name webshop2-staging \
              --restart unless-stopped \
              -p 8081:8080 \
              -e ASPNETCORE_URLS=http://+:8080 \
              -e ConnectionStrings__DefaultConnection="$DB_CONNECTION" \
              "$IMAGE"
            docker logout ghcr.io

  deploy_production:
    name: Deploy to Production
    runs-on: ubuntu-latest
    needs: docker
    if: github.event_name == 'workflow_dispatch' && inputs.deploy_production == true
    environment: production

    steps:
      - name: Deploy production
        uses: appleboy/ssh-action@v1.2.2
        env:
          IMAGE: ${{ env.IMAGE_NAME }}:sha-${{ github.sha }}
          REGISTRY_USER: ${{ secrets.GHCR_USERNAME }}
          REGISTRY_TOKEN: ${{ secrets.GHCR_TOKEN }}
          DB_CONNECTION: ${{ secrets.PRODUCTION_DB_CONNECTION }}
        with:
          host: ${{ secrets.PRODUCTION_HOST }}
          username: ${{ secrets.PRODUCTION_USER }}
          key: ${{ secrets.PRODUCTION_SSH_KEY }}
          envs: IMAGE,REGISTRY_USER,REGISTRY_TOKEN,DB_CONNECTION
          script: |
            set -e
            echo "$REGISTRY_TOKEN" | docker login ghcr.io \
              -u "$REGISTRY_USER" --password-stdin
            docker pull "$IMAGE"
            docker stop webshop2 || true
            docker rm webshop2 || true
            docker run -d \
              --name webshop2 \
              --restart unless-stopped \
              -p 8080:8080 \
              -e ASPNETCORE_URLS=http://+:8080 \
              -e ConnectionStrings__DefaultConnection="$DB_CONNECTION" \
              "$IMAGE"
            docker logout ghcr.io