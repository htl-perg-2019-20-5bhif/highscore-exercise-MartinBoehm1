docker network create spaceshooter
docker run 8083:80 --net=spaceshooter spaceshooterfrontend
docker run -p 5000:5000 --net=spaceshooter spaceshooterbackend