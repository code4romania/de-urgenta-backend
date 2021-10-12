cd /root/de-urgenta-backend/Src
git stash
git pull
git stash pop
docker-compose build
docker-compose down
docker-compose up -d
