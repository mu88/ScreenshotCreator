#!/bin/bash

apt-get update
apt-get install -y wget libssl1.1 libunwind8
mkdir -p /opt/microsoft/powershell/7
wget -O /tmp/powershell.tar.gz https://github.com/PowerShell/PowerShell/releases/download/v7.2.6/powershell-7.2.6-linux-arm64.tar.gz
tar zxf /tmp/powershell.tar.gz -C /opt/microsoft/powershell/7
chmod +x /opt/microsoft/powershell/7/pwsh
ln -s /opt/microsoft/powershell/7/pwsh /usr/bin/pwsh
rm /tmp/powershell.tar.gz