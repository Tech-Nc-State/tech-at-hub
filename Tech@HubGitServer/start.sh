#!/bin/bash

service nginx start
service fcgiwrap start

# allow nginx service to access the fcgiwrap socket
chmod 666 /var/run/fcgiwrap.socket

# give git user access to git folder
cd /app/git
chown -R www-data:www-data .
chmod -R 755 .
cd /app


# infinite loop, this is hacky and we really should
# try to monitor the nginx process if possible
while true; do sleep 1000; done
