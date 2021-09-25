#!/usr/bin/env bash

service nginx start
service fcgiwrap start

# allow nginx service to access the fcgiwrap socket
chmod 666 /var/run/fcgiwrap.socket

# create a server side git repository
#cd /app/git
#mkdir test.git
#cd test.git
#git --bare init
#git update-server-info
#git config http.receivepack true
# possibly need to create a dummy file here to make the clone work
#chown -R www-data:www-data .
#chmod -R 755 .
#cd /app

# infinite loop, this is hacky and we really should
# try to monitor the nginx process if possible
while true; do sleep 1000; done