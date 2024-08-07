#!/bin/bash
# Number of attempts
WAIT=10
# To keep count of how many attempts have passed
count=0

docker-compose up -d

while ! docker exec src_postgres_1 pg_isready -q  > /dev/null 2> /dev/null; do
    sleep 2

    if [ $count -gt $WAIT ]; then
        docker-compose down
        docker-compose up &
        count=0
    fi
    ((count++))
done

docker exec src_postgres_1 psql -U postgres -f /CREATE/create-database.sql
docker exec src_postgres_1 psql -U postgres -d matchstats -f /CREATE/create-tables.sql

docker exec src_postgres_1 psql -U postgres -c "alter user postgres with password 'password';" &
