#! /usr/bin/env sh
set -x

DEPLOY_DIR=$(dirname "$0")
SOURCE_DIR="${DEPLOY_DIR}/../"

cd $SOURCE_DIR \
    && git pull \
    && docker-compose down \
    && docker-compose build \
    && docker-compose up -d