docker build -t telegrambot .

rm -rf ~/image.tar
docker save -o ~/image.tar telegrambot
ssh admin@109.196.164.182 'rm -rf ~/image.tar'
scp ~/image.tar admin@109.196.164.182:~/ 
ssh admin@109.196.164.182 'docker load -i ~/image.tar'

## run on machine:
# docker run -d --restart unless-stopped telegrambot