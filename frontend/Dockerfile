FROM node:18-slim

WORKDIR /frontend

COPY package*.json ./

RUN npm install

RUN npm install esbuild

COPY . .

EXPOSE 5173

CMD ["npm", "run", "dev"]
