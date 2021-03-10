#!/bin/bash

curl -SL --output dotnet-install.sh https://dot.net/v1/dotnet-install.sh

DOTNET_VERSIONS=(
    # 'latest'
    '5.0.100'
)

for val in ${DOTNET_VERSIONS[@]}; do
   echo "installing $val"
   /bin/bash dotnet-install.sh --verbose --version $val
done