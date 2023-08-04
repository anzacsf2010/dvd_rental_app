#!/usr/bin/sh

cd /home/testing_user/apps/test_dotnet_app/dvd_rental_app || exit

export CORECLR_ENABLE_PROFILING=1
export CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A}
export CORECLR_NEWRELIC_HOME="/home/testing_user/apps/test_dotnet_app/dvd_rental_app/bin/Debug/net7.0/newrelic"
export CORECLR_PROFILER_PATH="/home/testing_user/apps/test_dotnet_app/dvd_rental_app/bin/Debug/net7.0/newrelic/libNewRelicProfiler.so"

export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet

dotnet run
