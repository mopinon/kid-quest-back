docker build -t mopin/kid-quest .
docker push mopin/kid-quest
ssh mopin <<'ENDSSH'
    cd /home/kid-quest/
    docker pull mopin/kid-quest
    docker-compose up -d
ENDSSH