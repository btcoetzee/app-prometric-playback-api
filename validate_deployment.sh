#!/bin/bash -

set -e
if [ $# -ne 3 ]; then
    echo "Got ${#} arguments instead of 3 arguments (deployment_name, build_id, namespace)"
    exit 1
fi

DEPLOYMENT_NAME=$1
BUILDID=$2
NAMESPACE=$3

# Test for deployment existence
retries=10

until [[ $(kubectl rollout status deployment/$DEPLOYMENT_NAME -n $NAMESPACE) || retries -eq 0 ]]
do
    echo waiting for rollout
    sleep 10
    ((retries--))
done

if [[ retries -eq 0 ]]
then
    echo Timed out waiting for pods 
    exit 1
else
    echo "$DEPLOYMENT_NAME:$BUILDID rollout to $NAMESPACE complete"
fi
