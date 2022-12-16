FROM node

RUN apt-get update    && apt-get install -y nginx
WORKDIR /app
COPY . /app/
EXPOSE 80
RUN  npm install --force\
    && npm run build \
    && cp -r dist/* /usr/share/nginx/html \
    && cp nginx.conf /etc/nginx/nginx.conf \
    && rm -rf /app
CMD ["nginx","-g","daemon off;"]