FROM mysql:latest

COPY a_init.sql /docker-entrypoint-initdb.d/a_init.sql
COPY b_load_data.sql /docker-entrypoint-initdb.d/b_load_data.sql
