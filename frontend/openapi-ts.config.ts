import { defineConfig } from '@hey-api/openapi-ts';
import { loadEnv } from 'vite';

const swaggerUrl = loadEnv('development', process.cwd()).VITE_SWAGGER_URL;

if (!swaggerUrl) {
    throw new Error('SWAGGER_URL environment variable is not defined');
}

export default defineConfig({
  input: swaggerUrl,
  output: {
    path: 'src/heyapi',
    postProcess: ['eslint', 'prettier'],
  },
  plugins: [
    {
      name: '@hey-api/sdk',
      auth: false,
      operations: {
        strategy: 'byTags',
        containerName: '{{name}}Service',
      },
    },
    {
      name: '@hey-api/client-fetch',
    },
    {
      name: '@hey-api/typescript',
      enums: 'typescript',
    },
  ]
});