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
FROM node:latest AS build
WORKDIR /usr/src/app
COPY . .
RUN npm install
RUN npm run-script build
 
### STAGE 2: Run ###
FROM nginx:alpine
COPY --from=build /usr/src/app/dist /usr/share/nginx/html