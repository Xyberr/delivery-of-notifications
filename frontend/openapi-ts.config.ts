import { defineConfig } from '@hey-api/openapi-ts';

const swaggerUrl = process.env.SWAGGER_URL;

if (!swaggerUrl) {
    throw new Error('SWAGGER_URL environment variable is not defined');
}

export default defineConfig({
  input: swaggerUrl,
  output: 'src/heyapi',
});