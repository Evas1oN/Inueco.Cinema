<script>
    const DEFAULT_GUID = "00000000-0000-0000-0000-000000000000"
    let selectedGenre = {
        id: "00000000-0000-0000-0000-000000000000",
        name: ""
    }

    let genres = []

    function submit(e)
    {   
        e.preventDefault()
        let method = selectedGenre.id == DEFAULT_GUID ? "POST" : "PUT"
        fetch("/api/genres", {
            method,
            body: JSON.stringify(selectedGenre),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(res => {
            if (res.ok) {
                if (method === "POST") res.json().then(data => genres = [...genres, data])
                else genres = genres

                reset()
                return;
            }
            throw(res.statusText)
        })
        .catch(err => console.error(err))
    }

    function remove(id) {
        fetch(`/api/genres/${id}`, { method: "delete" })
        .then(res => {
            if (res.ok) {
                genres = genres.filter(x => x.id != id)
                reset()
            }
        })
    }

    function reset() {
        selectedGenre = {
            id: DEFAULT_GUID,
            name: ""
        }
    }

    let awaiter = fetch("/api/genres")
    .then(res => res.json())
    .then(data => {
        genres = data
        return
    })
    .catch(err => {
        console.error(err)
    })
</script>

<br/>
<form on:submit={submit}>
    <label for="name">
        {#if selectedGenre.id != DEFAULT_GUID}
            <strong>{selectedGenre.id}</strong>
        {:else}
            Наименование
        {/if}
    </label>
    <input bind:value="{selectedGenre.name}"/>
    <button type="submit">Отправить</button>

    {#if selectedGenre.id != DEFAULT_GUID}
        <button type="reset" on:click={reset}>Сброс</button>
    {:else}
        <button disabled type="reset" on:click={reset}>Сброс</button>
    {/if}
</form>



{#await awaiter}
    <p>Loading...</p>
{:then data}
    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>Наименование</th>
                <th>Действие</th>
            </tr>
        </thead>
        <tbody>
            {#each genres as genre}
                <tr on:click={() => selectedGenre = genre}>
                    {#if selectedGenre == genre}
                        <td><strong>{genre.id}</strong></td>
                    {:else}
                       <td>{genre.id}</td>
                    {/if}
                    <td>{genre.name}</td>
                    <td><button on:click="{() => remove(genre.id)}">Удалить</button></td>
                </tr>
            {/each}
        </tbody>
    </table>
{/await}