# Helpful commands

- Run docker compose for specific file: `docker compose -f .\docker-compose-database.yml up`
- Rebuild docker compose without and cache: `docker compose -f .\docker-compose-database.yml build --no-cache`
- Run docker compose for specific file detached: `docker compose -f .\docker-compose-database.yml up -d`