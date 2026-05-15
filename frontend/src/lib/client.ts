import createFetchClient from "openapi-fetch";
import createClient from "openapi-react-query";
import type { paths } from "../types/schema";


const fetchClient = createFetchClient<paths>({
  baseUrl: "http://localhost:5131",
});
const $api = createClient(fetchClient);
export default $api;
