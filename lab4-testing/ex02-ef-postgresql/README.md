# Lab 04 - Testing and Performance

## Smoke Testing Report for Ex02

This report compares the performance of the `/persons` endpoint with and without `IMemoryCache`.

### Performance Comparison

| Endpoint | Average Duration | p(95) Duration | Max Duration |
|----------|-----------------|----------------|--------------|
| `/persons` (No Cache) | 35.53ms | 20.61ms | 902.02ms |
| `/persons/cached` | 2.04ms | 5.81ms | 157.30ms |

### Analysis

The k6 smoke test results confirm a significant performance boost when using `IMemoryCache`:
- **Average Latency**: The cached endpoint is **~17x faster** on average (2.04ms vs 35.53ms).
- **Consistency**: The `p(95)` for cached requests is much lower (5.81ms), and the maximum recorded latency for a cached hit (157.3ms) is drastically lower than the non-cached peak (902.02ms), which likely included database connection overhead or cold starts.
- **Throughput**: serving 5800 requests in 30 seconds with 100 VUs shows the system is stable under moderate load.
- **Resources**: serving from memory eliminates the need for repetitive PostgreSQL queries and AutoMapper transformations for static data, significantly reducing CPU and I/O pressure on the database.

### Smoke Test Technical Details (Actual Run)

- **Total Requests**: 5800
- **Success Rate**: 100% (0 errors)
- **VUs**: 100
- **Throughput**: ~192 reqs/s


The smoke tests were performed using `k6` with the following configuration:
- **VUs**: 100
- **Duration**: 30s
- **Checks**: p(95) response time < 1000ms

The test scripts are located in `ex02-ef-postgresql/tests/smoke/`.
