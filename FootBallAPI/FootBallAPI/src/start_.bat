@echo off

:: Run docker compose
docker-compose up -d

:: Check if container is ready
set index=1
:While
if %index% gtr 50 goto EndWhile
    docker exec src_postgres_1 pg_isready -q
    if NOT ERRORLEVEL 1 goto EndWhile
    
    timeout /t 2
    set /A index+=1
    goto While
:EndWhile

docker exec src_postgres_1 psql -U postgres -f /CREATE/create-database.sql
docker exec src_postgres_1 psql -U postgres -d matchstats -f /CREATE/create-tables.sql

@start /b cmd /c docker exec src_postgres_1 psql -U postgres -c "alter user postgres with password 'password';"
