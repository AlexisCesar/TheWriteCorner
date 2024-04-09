FROM openresty/openresty:bionic

RUN luarocks install lua-resty-jwt
COPY ./config_files /etc/nginx/conf.d
COPY ./jwt-auth.lua /usr/local/openresty/lualib/resty/jwt-auth.lua