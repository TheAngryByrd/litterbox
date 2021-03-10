FROM debian:buster-slim


RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
        # .NET Core dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu63 \
        libssl1.1 \
        libstdc++6 \
        zlib1g \
        curl \
        git \
        procps \
        wget \
    && rm -rf /var/lib/apt/lists/*


ENV \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_INSTALL_DIR=/usr/share/dotnet/ \
    DOTNET_ROOT=/usr/share/dotnet/

# # Install .NET SDK
# RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm64.tar.gz \
#     && mkdir -p /usr/share/dotnet \
#     && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz ./packs ./sdk ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
#     && rm dotnet.tar.gz \
#     # Trigger first run experience by running arbitrary cmd
#     && dotnet help

COPY ./install-dotnets.sh ./install-dotnets.sh

RUN /bin/bash install-dotnets.sh

ENV PATH="$DOTNET_ROOT:${PATH}"
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

RUN  dotnet --info

# Copy endpoint specific user settings into container to specify
# .NET Core should be used as the runtime.
COPY settings.vscode.json /root/.vscode-remote/data/Machine/settings.json
