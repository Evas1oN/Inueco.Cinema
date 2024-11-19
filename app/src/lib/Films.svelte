<script>
    const DEFAULT_GUID = "00000000-0000-0000-0000-000000000000"

    let selectedFilm =  {
        id: DEFAULT_GUID,
        name: "",
        genreId: DEFAULT_GUID,
        posterUrl: ""
    }

    let genres = fetch("/api/genres")
    .then(res => res.json())
    .catch(err => console.error(err))

    let films = load()
    async function load() {
        let response = await fetch("/api/films")
        if (!response.ok) {
            console.error(response.statusText)
            return
        }   
        return response.json()
    }

    /**
   * @param {Event} e
   */
    function submit(e)
    {
        e.preventDefault()
        let method = selectedFilm.id === DEFAULT_GUID ? "POST" : "PUT"

        fetch("/api/films", 
        { 
            method,
            body: JSON.stringify(selectedFilm),
            headers: {
                "Content-Type": "application/json"
            }
        }).then(res => {
            if (res.ok) {
                films = load()
                reset()
            }
        })
    }

    function remove(id) {
        fetch(`/api/films/${id}`, { method: "DELETE" })
        .then(res => {
            if (res.ok) {
                films = load()
                reset()
            }
        })
    }

    function reset() {
        selectedFilm =  {
            id: DEFAULT_GUID,
            name: "",
            genreId: DEFAULT_GUID,
            posterUrl: ""
        }
    }
</script>


<form on:submit={submit}>
    <label for="name">Наименование</label>
    <fieldset role="group">
        <input bind:value={selectedFilm.name}/>
        {#if selectedFilm.id != DEFAULT_GUID}
            <button type="reset" on:click={reset}>X</button>
        {/if}
    </fieldset>


    <div class="grid">
        <label>Жанр
            {#await genres}
                <article aria-busy="true"></article>
            {:then data} 
                <select bind:value={selectedFilm.genreId}>
                    {#each data as genre}
                        <option value={genre.id}>{genre.name}</option>
                    {/each}
                </select>
            {/await}
        </label>
        
        <label>
            Постер (URL)
            <input bind:value={selectedFilm.posterUrl} type="url"/>
        </label>
    </div>



    <img style="max-height: 10rem;" alt="poster here" src="{selectedFilm.posterUrl}"/>
    <button type="submit">Отправить</button>
</form>

{#await films}
    <article aria-busy="true"></article>
{:then data} 
    <table>
        <thead>
            <tr>
                <!-- <th>Id</th> -->
                <th>Постер</th>
                <th>Наименование</th>
                <th>Жанр</th>
                <th>Дейсвтия</th>
            </tr>
        </thead>

        <tbody>
            {#each data as film}
                <tr on:click={() => selectedFilm = film}>
                    <td><img style="max-height: 10rem;" src="{film.posterUrl}" alt="Poster"/></td>
                    <td>{film.name}</td>
                    <td>{film.genre.name}</td>
                    <td><button on:click={() => remove(film.id)}>Удалить</button></td>
                </tr>
            {/each}
        </tbody>
    </table>
{/await}

