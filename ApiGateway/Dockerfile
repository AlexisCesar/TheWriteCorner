FROM nginx:latest

RUN apt-get update && apt-get install -y \
    wget \
    gcc \
    make \
    libc-dev \
    libpcre3-dev \
    zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*

RUN wget http://nginx.org/download/nginx-1.21.5.tar.gz && \
    tar -zxvf nginx-1.21.5.tar.gz

RUN cd nginx-1.21.5 && \
    ./configure --with-http_auth_request_module && \
    make && \
    make install

COPY ./config_files /etc/nginx/conf.d

CMD ["nginx", "-g", "daemon off;"]
