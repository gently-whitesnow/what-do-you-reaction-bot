rm -rf ./publish
dotnet publish -c release -r ubuntu.16.04-x64 --self-contained
ssh admin@109.196.164.182 'rm -rf ~/publish'
scp ./publish admin@109.196.164.182:~/ 
ssh admin@109.196.164.182 'docker load -i /build/appname'