#---- Stage 1: Build ----
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS builder

# Install UPX for binary compression AND AOT compilation dependencies
RUN apk add --no-cache upx clang build-base zlib-dev

WORKDIR /app
COPY Program.cs smallestsharp.csproj ./

# Publish with AOT compilation for smallest size
RUN dotnet publish -c Release -o /out \
    -p:PublishAot=true \
    -p:PublishTrimmed=true \
    -p:TrimMode=full \
    -p:InvariantGlobalization=true \
    -p:OptimizationPreference=Size \
    -p:EventSourceSupport=false \
    -p:UseSystemResourceKeys=true \
    -p:DebugType=none \
    -p:DebugSymbols=false \
    -p:ErrorOnDuplicatePublishOutputFiles=false \
    --self-contained true \
    -r linux-musl-x64 \
    -p:EnableCompressionInSingleFile=true \
    -p:IlcGenerateCompleteTypeMetadata=false \
    -p:IlcOptimizationPreference=Size \
    -p:IlcFoldIdenticalMethodBodies=true

# Compress with UPX (aggressive compression)
RUN upx --ultra-brute /out/smallestsharp

#---- Stage 2: Minimal runtime image ----
FROM scratch

# Copy only the absolutely necessary runtime components
COPY --from=builder /lib/ld-musl-x86_64.so.1 /lib/

# Copy the compressed binary
COPY --from=builder /out/smallestsharp /app

EXPOSE 8080
ENTRYPOINT ["/app"]
