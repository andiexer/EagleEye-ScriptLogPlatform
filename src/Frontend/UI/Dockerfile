#alpine image
FROM nginx:alpine
#create the server and location configuration
COPY docker/default.conf /etc/nginx/conf.d/default.conf
COPY docker/setAppconfig.sh /usr/local/bin/setAppconfig.sh
#copies the build app to the default location
COPY dist /var/www
ENV ApiUrl=http://localhost:5000/api/
ENTRYPOINT ["/bin/sh", "/usr/local/bin/setAppconfig.sh"]