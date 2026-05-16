import React, { useRef } from "react";
import $api from "./lib/client";
import Button from "./components/button";

function App() {

  const { mutate, isPending, data, error } = $api.useMutation("post", "/api/LinkItems",
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

        {error && (
          <p className="rounded-xl bg-red-500/15  text-red-700 px-4 py-3">
            {String(error)}
          </p>
        )}
        {data?.url && (
          <a
          className="rounded-xl bg-green-500/15  text-green-700 px-4 py-3 underline hover:text-green-800"
          href={data?.url}>
            {data?.url}
          </a>
        )}
        <a className=""></a>

        <input placeholder="https://example-long-link.com" ref={urlRef} className=" p-2 text-2xl rounded-xl border border-stone-200 tracking-tight"></input>
        <Button label="Submit" />

      </form>
    </div>
  )
}

export default App
