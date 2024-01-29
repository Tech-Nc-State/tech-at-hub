FROM ubuntu:latest
EXPOSE 80

# set eastern timezone
ENV TZ=America/New_York

COPY . /app
WORKDIR /app

RUN apt-get clean && apt-get -y update

RUN apt-get -y install nginx git-core fcgiwrap

COPY nginx.conf /etc/nginx
RUN chmod +x ./start.sh
CMD ["./start.sh"]
