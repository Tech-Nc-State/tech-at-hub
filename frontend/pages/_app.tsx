import '../styles/globals.css'
import type { AppProps } from 'next/app'
import {GlobalServices} from "../services/GlobalServices";

function MyApp({ Component, pageProps }: AppProps) {
  return (
      <GlobalServices>
        <Component {...pageProps} />
      </GlobalServices>

  )
}

export default MyApp
