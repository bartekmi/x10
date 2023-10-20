// Note: This file must have the name 'codegen.ts'
// TBD how to change so that 'graphql-codegen' can find it.
import { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
  schema: '../../../x10/SmallSample.graphql',
  documents: ['src/**/*.{ts,tsx}'],
  generates: {
    './src/__generated__/': {
      preset: 'client',
      plugins: [],
      presetConfig: {
        gqlTagName: 'gql',
      }
    }
  },
  ignoreNoDocuments: false,
};

export default config;