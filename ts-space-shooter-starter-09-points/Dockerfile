#FROM node:alpine
#WORKDIR /app
#ENV PORT 80
#EXPOSE 80
#
#COPY . .
#RUN npm install && npm run build
#FROM nginx:alpine
#COPY /app/dist /usr/src/app
#ENV PORT 8081
#EXPOSE $PORT
#WORKDIR /usr/src/app
#CMD ["npm", "start"]



### STAGE 1: Build ###
FROM node:alpine AS build
WORKDIR /usr/src/app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build
 
### STAGE 2: Run ###
FROM nginx:alpine
COPY --from=build /usr/src/app/dist /usr/share/nginx/html