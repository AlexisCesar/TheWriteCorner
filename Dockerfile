FROM mongo
COPY scripts/create-articles-management-db.js /docker-entrypoint-initdb.d/
