import React, { useRef } from "react";
import $api from "./lib/client";
import Button from "./components/button";

function App() {

  const { mutate, isPending, data,error } = $api.useMutation("post", "/api/LinkItems",
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
    <div className="flex flex-col w-dvw h-dvh justify-center items-center">
      <form onSubmit={e => HandleSubmit(e)}
        className="shadow-2xl
        w-fit
        h-fit
        p-5
        flex
        flex-col
        items-center
        gap-4
        border-2
      border-stone-100
        "
        >
        <h1 className="text-2xl">Shorten a long link</h1>
        <input placeholder="https://example-long-link.com" ref={urlRef} className=" p-2 text-2xl rounded-xl border border-stone-200"></input>
        <Button label="Submit"/>

        <h1>{error}</h1>
        <h1>{data?.url}</h1>
      </form>
    </div>
  )
}

export default App
