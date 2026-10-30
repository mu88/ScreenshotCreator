#!/bin/bash
set -euo pipefail

# SHA256 published by Microsoft for this exact release asset:
# https://github.com/PowerShell/PowerShell/releases/tag/v7.4.0
readonly POWERSHELL_SHA256="13d317610ba133c002990c40622a886989368460451809861f378c25730989f4"

apt-get update
apt-get install -y wget libssl3 libunwind8
mkdir -p /opt/microsoft/powershell/7
wget -O /tmp/powershell.tar.gz https://github.com/PowerShell/PowerShell/releases/download/v7.4.0/powershell-7.4.0-linux-arm64.tar.gz
# Verifying the checksum defends against a tampered or redirected download (docker/shell:S6506),
# regardless of where the request was ultimately served from.
echo "${POWERSHELL_SHA256}  /tmp/powershell.tar.gz" | sha256sum --check --status
tar zxf /tmp/powershell.tar.gz -C /opt/microsoft/powershell/7
chmod +x /opt/microsoft/powershell/7/pwsh
ln -s /opt/microsoft/powershell/7/pwsh /usr/bin/pwsh
rm /tmp/powershell.tar.gz