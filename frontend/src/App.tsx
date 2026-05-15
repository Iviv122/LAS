import { useRef } from "react";
import $api from "./lib/client";

function App() {

  const { mutate, isPending, data } = $api.useMutation("post","/api/LinkItems",
    {
      onError: (err) => {console.log({err})}
    },
  );

  const urlRef = useRef<HTMLInputElement>(null);

  function HandleSubmit(e : React.SubmitEvent<HTMLFormElement>) {
    e.preventDefault()
    if (urlRef.current) {
      mutate({
        body:{
          url: urlRef.current.value
        }
      })
    }
  }

  if(isPending) return <p>Loading...</p>

  return (
    <>
      <form onSubmit={e =>HandleSubmit(e)}>
        <input ref={urlRef} className="border"></input>
        <button type="submit">Send</button>
      </form>
      <h1>{data?.url}</h1>
    </>
  )
}

export default App
