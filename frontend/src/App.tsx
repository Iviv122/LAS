import { useRef } from "react";
import $api from "./lib/client";

function App() {

  const { mutate, isPending, data } = $api.useMutation("post", "/api/LinkItems",
    {
      onError: (err) => { console.log({ err }) }
    },
  );

  const urlRef = useRef<HTMLInputElement>(null);

  function HandleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
    e.preventDefault()
    if (urlRef.current) {
      mutate({
        body: {
          url: urlRef.current.value
        }
      })
    }
  }

  if (isPending) return <p>Loading...</p>

  return (
    <div className="flex w-dvh h-dvh ">
      <form onSubmit={e => HandleSubmit(e)}
        className="shadow-2xl w-fit h-fit p-5 flex flex-col"
        >
        <h1>Shorten a long link</h1>
        <input placeholder="https://examplelonglink.com" ref={urlRef} className="border"></input>
      </form>
      <h1>{data?.url}</h1>
    </div>
  )
}

export default App
